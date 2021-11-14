using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Entities.Common;
using MongoDB.Driver;

namespace ChatApp.DataAccess
{
    public interface IRepository<TDocument> where TDocument : BaseModel
    {
        IMongoCollection<TDocument> Collection { get; }

        Task<IEnumerable<TDocument>> FindAll();

        Task<IEnumerable<TDocument>> Find(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument> options = null);

        Task<long> Count(FilterDefinition<TDocument> filter, CountOptions options = null);

        Task<TDocument> First(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument> options = null);

        Task<TDocument> FirstOrDefault(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument> options = null);

        Task Insert(TDocument entity);

        Task Insert(IEnumerable<TDocument> entities);

        Task Update(TDocument entity);

        Task Update(IEnumerable<TDocument> entities);

        Task UpdatePartial(TDocument entity, UpdateDefinition<TDocument> toUpdate);

        Task DeleteSoft(TDocument entity);

        Task DeleteSoft(IEnumerable<TDocument> entities);

        Task Delete(TDocument entity);

        Task Delete(IEnumerable<TDocument> entities);
    }
}
