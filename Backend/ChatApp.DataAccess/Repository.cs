using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Entities.Common;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace ChatApp.DataAccess
{
    public class Repository<TDocument> : IRepository<TDocument> where TDocument : BaseModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUserId => _httpContextAccessor.HttpContext?.Items[RequestKeys.UserId]?.ToString()
                                        ?? GlobalConstants.DefaultUser;

        public IMongoCollection<TDocument> Collection { get; }

        public Repository(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _httpContextAccessor = httpContextAccessor;

            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;
            var database = client.GetDatabase(databaseName);

            Collection = database.GetCollection<TDocument>(MongoCollectionNames.Get(typeof(TDocument).Name));
        }

        public async Task<long> Count(FilterDefinition<TDocument> filter, CountOptions options = null)
        {
            filter &= MongoExtension.GetBuilders<TDocument>().Ne(x => x.IsDeleted, true);
            return await Collection.CountDocumentsAsync(filter, options);
        }

        public async Task<TDocument> First(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument> options = null)
        {
            filter &= MongoExtension.GetBuilders<TDocument>().Ne(x => x.IsDeleted, true);
            return await (await Collection.FindAsync(filter, options)).FirstAsync();
        }

        public async Task<TDocument> FirstOrDefault(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument> options = null)
        {
            filter &= MongoExtension.GetBuilders<TDocument>().Ne(x => x.IsDeleted, true);
            return await (await Collection.FindAsync(filter, options)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDocument>> Current(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument> options = null)
        {
            filter &= MongoExtension.GetBuilders<TDocument>().Ne(x => x.IsDeleted, true);
            return (await Collection.FindAsync(filter, options)).Current;
        }

        public async Task Insert(TDocument entity)
        {
            entity.CreatedBy = CurrentUserId;
            entity.CreatedAt = DateTimeExtension.Get();
            await Collection.InsertOneAsync(entity);
        }

        public async Task Insert(IEnumerable<TDocument> entities)
        {
            var entitiesAsList = entities.ToList();
            foreach (var entity in entitiesAsList)
            {
                entity.CreatedBy = CurrentUserId;
                entity.CreatedAt = DateTimeExtension.Get();
            }
            await Collection.InsertManyAsync(entitiesAsList);
        }

        public async Task Update(TDocument entity)
        {
            entity.UpdatedBy = CurrentUserId;
            entity.UpdatedAt = DateTimeExtension.Get();
            await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task Update(IEnumerable<TDocument> entities)
        {
            await Task.WhenAll(entities.Select(async entity => await Update(entity)));
        }

        public async Task UpdatePartial(TDocument entity, UpdateDefinition<TDocument> toUpdate)
        {
            toUpdate = toUpdate.Set(e => e.UpdatedBy, CurrentUserId);
            toUpdate = toUpdate.Set(e => e.UpdatedAt, DateTimeExtension.Get());
            await Collection.UpdateOneAsync(e => e.Id == entity.Id, toUpdate);
        }

        public async Task DeleteSoft(TDocument entity)
        {
            var toUpdateBuilder = new UpdateDefinitionBuilder<TDocument>();
            var toUpdate = toUpdateBuilder.Set(e => e.IsDeleted, true);

            await UpdatePartial(entity, toUpdate);
        }

        public async Task DeleteSoft(IEnumerable<TDocument> entities)
        {
            await Task.WhenAll(entities.Select(async entity => await DeleteSoft(entity)));
        }

        public async Task Delete(TDocument entity)
        {
            await Collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task Delete(IEnumerable<TDocument> entities)
        {
            await Task.WhenAll(entities.Select(async entity => await Delete(entity)));
        }
    }
}
