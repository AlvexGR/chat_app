﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
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
using ChatApp.Entities.Enums;
using ChatApp.Utilities.Extensions;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ChatApp.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger,
            IMapper mapper,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
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

            var jwtSetting = GetJwtSetting();

            var loginResponseDto = new LoginResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                Token = user.GenerateAccessToken(jwtSetting)
            };

            return new BaseResponseDto<LoginResponseDto>().GenerateSuccessResponse(loginResponseDto);
        }

        public async Task<BaseResponseDto<LoginResponseDto>> Register(RegisterDto registerDto)
        {
            if (registerDto == null ||
                string.IsNullOrEmpty(registerDto.Email) ||
                string.IsNullOrEmpty(registerDto.Password))
            {
                return new BaseResponseDto<LoginResponseDto>().GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            var userRepo = _unitOfWork.GetRepository<User>();

            var userQuery = MongoExtension.GetBuilders<User>()
                .Regex(x => x.Email, registerDto.Email.MongoIgnoreCase());

            var existed = await userRepo.Count(userQuery);
            if (existed != 0)
            {
                return new BaseResponseDto<LoginResponseDto>().GenerateFailedResponse(ErrorCodes.EmailExists);
            }

            var user = _mapper.Map<User>(registerDto);

            user.FirstName = user.FirstName.Trim();
            user.LastName = user.LastName.Trim();
            user.Email = user.Email.Trim();
            user.Password = user.Password.HashMd5();
            user.ConfirmationToken = Guid.NewGuid().ToString().HashMd5();
            user.Role = UserRole.User;

            await userRepo.Insert(user);

            await _emailService.Send(new EmailDto
            {
                Title = EmailTemplates.ConfirmAccountTitle,
                Address = user.Email,
                Content = EmailTemplates.ConfirmAccountBody
                    .Replace("#name#", user.FirstName)
                    .Replace("#link#", $"{_configuration[AppSettingKeys.FrontEndHost]}/confirm-account/{user.ConfirmationToken}")
            });

            return await Login(new LoginRequestDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            });
        }

        public async Task<BaseResponseDto<LoginResponseDto>> GoogleLogin(GoogleLoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null || string.IsNullOrEmpty(loginRequestDto.Token))
            {
                return new BaseResponseDto<LoginResponseDto>()
                    .GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            var payload = await GoogleJsonWebSignature.ValidateAsync(loginRequestDto.Token);

            var jwtSetting = GetJwtSetting();

            var userRepo = _unitOfWork.GetRepository<User>();

            var userQuery = MongoExtension.GetBuilders<User>()
                .Regex(x => x.Email, payload.Email.MongoIgnoreCase());

            var user = await userRepo.FirstOrDefault(userQuery);

            LoginResponseDto loginResponseDto;

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Role = UserRole.User,
                    GooglePassword = Guid.NewGuid().ToString().HashMd5()
                };

                await userRepo.Insert(user);

                loginResponseDto = new LoginResponseDto
                {
                    User = _mapper.Map<UserResponseDto>(user),
                    Token = user.GenerateAccessToken(jwtSetting, true)
                };

                return new BaseResponseDto<LoginResponseDto>().GenerateSuccessResponse(loginResponseDto);
            }

            loginResponseDto = new LoginResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                Token = user.GenerateAccessToken(jwtSetting, true)
            };

            return new BaseResponseDto<LoginResponseDto>().GenerateSuccessResponse(loginResponseDto);
        }

        public async Task<BaseResponseDto<bool>> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new BaseResponseDto<bool>()
                    .GenerateFailedResponse(ErrorCodes.BadRequest);
            }

            var userRepo = _unitOfWork.GetRepository<User>();

            var queryBuilder = MongoExtension.GetBuilders<User>();
            var userQuery = queryBuilder
                .Regex(x => x.Email, email.MongoIgnoreCase());
            var user = await userRepo.FirstOrDefault(userQuery);

            if (user == null)
            {
                return new BaseResponseDto<bool>()
                    .GenerateFailedResponse(ErrorCodes.EmailExists);
            }

            var newPassword = 8.GenerateRandomPassword();
            user.Password = newPassword.HashMd5();
            var updateDefinition = new UpdateDefinitionBuilder<User>()
                .Set(x => x.Password, user.Password);

            await userRepo.UpdatePartial(user, updateDefinition);

            var result = await _emailService.Send(new EmailDto
            {
                Title = EmailTemplates.ForgotPasswordTitle,
                Address = user.Email,
                Content = EmailTemplates.ForgotPasswordBody
                    .Replace("#name#", user.FirstName)
                    .Replace("#password#", newPassword)
            });

            return new BaseResponseDto<bool>().GenerateSuccessResponse(result);
        }

        public async Task<(JwtSecurityToken token, UserDto user)> VerifyAuthToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims
                .First(x => x.Type == UserClaimTypes.UserId)
                .Value;

            var user = await _unitOfWork
                .GetRepository<User>()
                .FindById(userId);
            if (user == null) throw new Exception("User is null");

            var isGoogleLogin = Convert.ToBoolean(jwtToken.Claims
                .First(x => x.Type == UserClaimTypes.IsGoogleLogin)
                .Value);

            var key = Encoding.ASCII.GetBytes(
                !isGoogleLogin
                    ? user.Password
                    : user.GooglePassword);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return (jwtToken, _mapper.Map<UserDto>(user));
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
