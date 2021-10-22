using System;
using System.Collections.Generic;
using System.Security.Claims;
using ChatApp.Entities.Common;
using ChatApp.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ChatApp.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly Dictionary<Type, object> _repositories;
        private readonly string _currentUser;

        public UnitOfWork(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _repositories = new Dictionary<Type, object>();
            _currentUser = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? GlobalConstants.DefaultUser;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel
        {
            var type = typeof(Repository<TEntity>);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_configuration.GetConnectionString("ChatAppDb"), _currentUser);
            }

            return (IRepository<TEntity>)_repositories[type];
        }
    }
}
