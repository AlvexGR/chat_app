using System;
using System.Collections.Generic;
using ChatApp.Entities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ChatApp.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TDocument> GetRepository<TDocument>() where TDocument : BaseModel
        {
            var type = typeof(Repository<TDocument>);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TDocument>(
                    _configuration.GetConnectionString("ChatAppDb"),
                    _httpContextAccessor);
            }

            return (IRepository<TDocument>)_repositories[type];
        }
    }
}
