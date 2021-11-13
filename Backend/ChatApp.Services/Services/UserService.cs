using System;
using System.Threading.Tasks;
using AutoMapper;
using ChatApp.DataAccess;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Auths;
using ChatApp.Dtos.Models.Emails;
using ChatApp.Dtos.Models.Users;
using ChatApp.Entities.Models;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Enums;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ChatApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            ILogger<UserService> logger,
            IMapper mapper,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<BaseResponseDto<bool>> Insert(InsertUserDto insertUserDto)
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

        public async Task<BaseResponseDto<ChangePasswordResponseDto>> ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            if (changePasswordRequestDto == null)
            {
                _logger.LogError("changePasswordRequestDto is null");
                return new BaseResponseDto<ChangePasswordResponseDto>().GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            if (string.IsNullOrEmpty(changePasswordRequestDto.NewPassword))
            {
                _logger.LogError("NewPassword is null");
                return new BaseResponseDto<ChangePasswordResponseDto>().GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            if (changePasswordRequestDto.CurrentPassword == changePasswordRequestDto.NewPassword)
            {
                _logger.LogError("NewPassword is the same as current password");
                return new BaseResponseDto<ChangePasswordResponseDto>().GenerateFailedResponse(ErrorCodes.SameNewPassword);
            }

            var hashCurrentPassword = changePasswordRequestDto.CurrentPassword.HashMd5();

            var userId = _httpContextAccessor.HttpContext.UserId();

            var builder = MongoExtension.GetBuilders<User>();
            var userRepo = _unitOfWork.GetRepository<User>();

            var user = await userRepo.FirstOrDefault(builder.Eq(x => x.Id, userId));
            if (user == null)
            {
                _logger.LogError("User is null");
                return new BaseResponseDto<ChangePasswordResponseDto>().GenerateFailedResponse(ErrorCodes.NotFound);
            }

            if (!user.IsConfirmed)
            {
                return new BaseResponseDto<ChangePasswordResponseDto>().GenerateFailedResponse(ErrorCodes.AccountHasNotBeenConfirmed);
            }

            if (!_httpContextAccessor.HttpContext.IsGoogleLogin() && user.Password != hashCurrentPassword)
            {
                _logger.LogError("Incorrect password");
                return new BaseResponseDto<ChangePasswordResponseDto>().GenerateFailedResponse(ErrorCodes.IncorrectCurrentPassword);
            }

            var hashNewPassword = changePasswordRequestDto.NewPassword.HashMd5();
            user.Password = hashNewPassword;

            var updateDefinition = new UpdateDefinitionBuilder<User>()
                .Set(x => x.Password, user.Password);

            await userRepo.UpdatePartial(user, updateDefinition);

            var jwtSetting = GetJwtSetting();

            return new BaseResponseDto<ChangePasswordResponseDto>().GenerateSuccessResponse(new ChangePasswordResponseDto
            {
                Token = user.GenerateAccessToken(jwtSetting)
            });
        }

        public async Task<BaseResponseDto<bool>> SendAccountConfirmation()
        {
            var userId = _httpContextAccessor.HttpContext.UserId();
            var userRepo = _unitOfWork.GetRepository<User>();

            var userQuery = MongoExtension.GetBuilders<User>()
                .Regex(x => x.Id, userId);

            var user = await userRepo.FirstOrDefault(userQuery);
            if (user == null)
            {
                _logger.LogError("user is null");
                return new BaseResponseDto<bool>().GenerateFailedResponse(ErrorCodes.NotFound);
            }

            if (user.IsConfirmed)
            {
                _logger.LogError("user is confirmed");
                return new BaseResponseDto<bool>().GenerateSuccessResponse(true);
            }

            if (string.IsNullOrEmpty(user.ConfirmationToken))
            {
                user.ConfirmationToken = Guid.NewGuid().ToString().HashMd5();
                var updateDefinition = new UpdateDefinitionBuilder<User>()
                    .Set(x => x.ConfirmationToken, user.ConfirmationToken);

                await userRepo.UpdatePartial(user, updateDefinition);
            }

            var result = await _emailService.Send(new EmailDto
            {
                Title = EmailTemplates.ConfirmAccountTitle,
                Address = user.Email,
                Content = EmailTemplates.ConfirmAccountBody
                    .Replace("#name#", user.FirstName)
                    .Replace("#link#", $"{_configuration[AppSettingKeys.FrontEndHost]}/confirm-account/{user.ConfirmationToken}")
            });

            return new BaseResponseDto<bool>().GenerateSuccessResponse(result);
        }

        public async Task<BaseResponseDto<LoginResponseDto>> ConfirmAccount(string token)
        {
            var userRepo = _unitOfWork.GetRepository<User>();

            var userQuery = MongoExtension.GetBuilders<User>()
                .Regex(x => x.ConfirmationToken, token);

            var user = await userRepo.FirstOrDefault(userQuery);

            if (user == null)
            {
                _logger.LogError($"Unable to find any user with this confirmation token: {token}");
                return new BaseResponseDto<LoginResponseDto>()
                    .GenerateFailedResponse(ErrorCodes.NotFound);
            }

            var jwtSetting = GetJwtSetting();

            LoginResponseDto loginResponseDto;

            if (user.IsConfirmed)
            {
                loginResponseDto = new LoginResponseDto
                {
                    User = _mapper.Map<UserResponseDto>(user),
                    Token = user.GenerateAccessToken(jwtSetting, true)
                };

                return new BaseResponseDto<LoginResponseDto>().GenerateSuccessResponse(loginResponseDto);
            }

            user.IsConfirmed = true;
            user.ConfirmationToken = null;

            var updateDefinition = new UpdateDefinitionBuilder<User>()
                .Set(x => x.IsConfirmed, user.IsConfirmed)
                .Set(x => x.ConfirmationToken, user.ConfirmationToken);

            await userRepo.UpdatePartial(user, updateDefinition);

            loginResponseDto = new LoginResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                Token = user.GenerateAccessToken(jwtSetting)
            };

            return new BaseResponseDto<LoginResponseDto>().GenerateSuccessResponse(loginResponseDto);
        }

        private JwtSettingDto GetJwtSetting()
        {
            var jwtSetting = new JwtSettingDto();
            _configuration
                .GetSection(AppSettingKeys.JwtSettingSection)
                .Bind(jwtSetting);
            return jwtSetting;
        }
    }
}
