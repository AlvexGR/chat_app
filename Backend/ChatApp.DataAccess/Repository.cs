using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Entities.Common;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Extensions;
using MongoDB.Driver;

namespace ChatApp.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly string _currentUserId;

        public IMongoCollection<T> Collection { get; }

        public Repository(string connectionString, string currentUserId)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _currentUserId = currentUserId;

            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;
            var database = client.GetDatabase(databaseName);

            Collection = database.GetCollection<T>(MongoCollectionNames.Get(typeof(T).Name));
        }

        public async Task Insert(T entity)
        {
            entity.CreatedBy = _currentUserId;
            entity.CreatedAt = DateTimeExtension.Get();
            await Collection.InsertOneAsync(entity);
        }

        public async Task Insert(IEnumerable<T> entities)
        {
            var entitiesAsList = entities.ToList();
            foreach (var entity in entitiesAsList)
            {
                entity.CreatedBy = _currentUserId;
                entity.CreatedAt = DateTimeExtension.Get();
            }
            await Collection.InsertManyAsync(entitiesAsList);
        }

        public async Task Update(T entity)
        {
            entity.UpdatedBy = _currentUserId;
            entity.UpdatedAt = DateTimeExtension.Get();
            await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task Update(IEnumerable<T> entities)
        {
            await Task.WhenAll(entities.Select(async entity => await Update(entity)));
        }

        public async Task UpdatePartial(T entity, UpdateDefinition<T> toUpdate)
        {
            toUpdate = toUpdate.Set(e => e.UpdatedBy, _currentUserId);
            toUpdate = toUpdate.Set(e => e.UpdatedAt, DateTimeExtension.Get());
            await Collection.UpdateOneAsync(e => e.Id == entity.Id, toUpdate);
        }

        public async Task DeleteSoft(T entity)
        {
            var toUpdateBuilder = new UpdateDefinitionBuilder<T>();
            var toUpdate = toUpdateBuilder.Set(e => e.IsDeleted, true);

            await UpdatePartial(entity, toUpdate);
        }

        public async Task DeleteSoft(IEnumerable<T> entities)
        {
            await Task.WhenAll(entities.Select(async entity => await DeleteSoft(entity)));
        }

        public async Task Delete(T entity)
        {
            await Collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task Delete(IEnumerable<T> entities)
        {
            await Task.WhenAll(entities.Select(async entity => await Delete(entity)));
        }
    }
}
