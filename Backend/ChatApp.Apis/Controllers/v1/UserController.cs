using System;
using System.Threading.Tasks;
using ChatApp.Apis.Filters;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Users;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Enums;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatApp.Apis.Controllers.v1
{
    [Route("api/users/v1")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger,
            IUserService userService)
            : base(httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("insert")]
        [TypeFilter(typeof(RoleFilter), Arguments = new object[] { new[] { UserRole.Admin } })]
        public async Task<BaseResponseDto<bool>> Insert(InsertUserDto insertUserDto)
        {
            try
            {
                return await _userService.Insert(insertUserDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Insert user error: {ex}");
                return new BaseResponseDto<bool>()
                    .GenerateGeneralFailedResponse(ex.ToString());
            }
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<BaseResponseDto<ChangePasswordResponseDto>> ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            try
            {
                return await _userService.ChangePassword(changePasswordRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Change password error: {ex}");
                return new BaseResponseDto<ChangePasswordResponseDto>()
                    .GenerateGeneralFailedResponse(ex.ToString());
            }
        }
    }
}
