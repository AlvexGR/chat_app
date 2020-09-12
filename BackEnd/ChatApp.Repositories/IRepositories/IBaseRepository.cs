using ChatApp.Entities.Models;

namespace ChatApp.Repositories.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
    }
}
