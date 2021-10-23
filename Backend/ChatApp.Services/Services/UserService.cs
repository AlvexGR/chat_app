using System.Threading.Tasks;
using AutoMapper;
using ChatApp.DataAccess;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Users;
using ChatApp.Entities.Models;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Enums;
using ChatApp.Utilities.Extensions;
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

        public async Task<BaseResponseDto<bool>> InsertUser(InsertUserDto insertUserDto)
        {
            if (insertUserDto == null)
            {
                _logger.LogError("InsertUserDto is null");
                return new BaseResponseDto<bool>().GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            if (string.IsNullOrEmpty(insertUserDto.Email) ||
                string.IsNullOrEmpty(insertUserDto.Password))
            {
                _logger.LogError("Email or password is empty");
                return new BaseResponseDto<bool>().GenerateFailedResponse(ErrorCodes.BadRequest);
            }
            var userRepo = _unitOfWork.GetRepository<User>();

            var userQuery = MongoExtension.GetBuilders<User>()
                .Regex(x => x.Email, insertUserDto.Email.MongoIgnoreCase());

            var existed = await userRepo.Count(userQuery);
            if (existed != 0)
            {
                _logger.LogError($"Email exists: {insertUserDto.Email}");
                return new BaseResponseDto<bool>().GenerateFailedResponse(ErrorCodes.EmailExists);
            }

            var user = _mapper.Map<User>(insertUserDto);

            user.Password = user.Password.HashMd5();
            user.Role = UserRole.User.ToInt();

            await userRepo.Insert(user);

            _logger.LogInformation("Insert user success");

            return new BaseResponseDto<bool>().GenerateSuccessResponse(true);
        }
    }
}
