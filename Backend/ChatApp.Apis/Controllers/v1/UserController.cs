using System;
using System.Threading.Tasks;
using ChatApp.Apis.Filters;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Users;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Enums;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatApp.Apis.Controllers.v1
{
    [Route("users/v1")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("insert")]
        [TypeFilter(typeof(RoleFilter), Arguments = new object[] { new[] { UserRole.Admin } })]
        public async Task<BaseResponseDto<bool>> InsertUser(InsertUserDto insertUserDto)
        {
            try
            {
                return await _userService.InsertUser(insertUserDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Insert user error: {ex}");
                return new BaseResponseDto<bool>()
                    .GenerateGeneralFailedResponse(ex.ToString());
            }
        }
    }
}
