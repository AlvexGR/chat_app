using ChatApp.Entities.Common;

namespace ChatApp.DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel;
    }
}
