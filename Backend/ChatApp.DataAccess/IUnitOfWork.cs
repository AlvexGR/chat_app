using ChatApp.Entities.Common;

namespace ChatApp.DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<TDocument> GetRepository<TDocument>()
            where TDocument : BaseModel;
    }
}
