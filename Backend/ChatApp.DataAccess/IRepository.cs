using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Entities.Common;
using MongoDB.Driver;

namespace ChatApp.DataAccess
{
    public interface IRepository<T> where T : BaseModel
    {
        Task Insert(T entity);

        Task Insert(IEnumerable<T> entities);

        Task Update(T entity);

        Task Update(IEnumerable<T> entities);

        Task UpdatePartial(T entity, UpdateDefinition<T> toUpdate);

        Task DeleteSoft(T entity);

        Task DeleteSoft(IEnumerable<T> entities);

        Task Delete(T entity);

        Task Delete(IEnumerable<T> entities);
    }
}
