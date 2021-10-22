using System.Threading.Tasks;
using AutoMapper;
using ChatApp.DataAccess;
using ChatApp.Dtos.Models.Users;
using ChatApp.Entities.Models;
using ChatApp.Services.IServices;
using Microsoft.Extensions.Logging;

namespace ChatApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(ILogger<UserService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> InsertUser(InsertUserDto insertUserDto)
        {
            if (insertUserDto == null)
            {
                _logger.LogError("InsertUserDto is null");
                return false;
            }

            var user = _mapper.Map<User>(insertUserDto);

            // TODO: Hash password

            var userRepo = _unitOfWork.GetRepository<User>();
            await userRepo.Insert(user);

            _logger.LogInformation("Insert user succeeded");

            return true;
        }
    }
}
