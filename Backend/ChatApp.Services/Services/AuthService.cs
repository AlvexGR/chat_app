using System.Threading.Tasks;
using AutoMapper;
using ChatApp.DataAccess;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Auths;
using ChatApp.Dtos.Models.Users;
using ChatApp.Entities.Models;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ChatApp.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(ILogger<AuthService> logger, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<BaseResponseDto<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                return new BaseResponseDto<LoginResponseDto>()
                    .GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            if (string.IsNullOrEmpty(loginRequestDto.Email) ||
                string.IsNullOrEmpty(loginRequestDto.Password))
            {
                return new BaseResponseDto<LoginResponseDto>()
                    .GenerateFailedResponse(ErrorCodes.InvalidCredential);
            }

            var userRepo = _unitOfWork.GetRepository<User>();

            var queryBuilder = MongoExtension.GetBuilders<User>();
            var userQuery = queryBuilder
                .Regex(x => x.Email, loginRequestDto.Email.MongoIgnoreCase());
            var user = await userRepo.FirstOrDefault(userQuery);

            if (user == null || user.Password != loginRequestDto.Password.HashMd5())
            {
                return new BaseResponseDto<LoginResponseDto>()
                    .GenerateFailedResponse(ErrorCodes.InvalidCredential);
            }

            var jwtSetting = new JwtSettingDto();
            _configuration
                .GetSection(AppSettingKeys.JwtSettingSection)
                .Bind(jwtSetting);

            var loginResponseDto = new LoginResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                Token = user.GenerateAccessToken(jwtSetting)
            };

            return new BaseResponseDto<LoginResponseDto>().GenerateSuccessResponse(loginResponseDto);
        }
    }
}
