using System;
using System.Threading.Tasks;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Users;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatApp.Apis.Controllers.v1
{
    [ApiController]
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
        public async Task<BaseResponseDto<bool>> InsertUser(InsertUserDto insertUserDto)
        {
            var result = new BaseResponseDto<bool>();
            try
            {
                var data = await _userService.InsertUser(insertUserDto);
                return result.GenerateSuccessResponse(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Insert user error: {ex}");
                return result.GenerateGeneralFailedResponse(ex.ToString());
            }
        }
    }
}
