using ChatApp.Entities.Models;
using ChatApp.Repositories.IRepositories;

namespace ChatApp.Repositories.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
    {
    }
}
