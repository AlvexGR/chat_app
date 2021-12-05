using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Dtos.Models.Users;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Apis.Filters
{
    public class HubAuthFilter : IHubFilter
    {
        private readonly IAuthService _authService;

        public HubAuthFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            await Authorize(context.Context.GetHttpContext());
            await next(context);
        }

        public async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            return await next(invocationContext);
        }

        private async Task Authorize(HttpContext httpContext)
        {
            JwtSecurityToken token;
            UserDto user;

            try
            {
                var (jwtSecurityToken, userDto) = await _authService
                    .VerifyAuthToken(httpContext.Request
                        .Query[RequestKeys.AccessToken].ToString());

                token = jwtSecurityToken;
                user = userDto;
            }
            catch (Exception)
            {
                throw new HubException(ErrorCodes.Unauthorized);
            }

            var isGoogleLogin = Convert.ToBoolean(token.Claims
                .First(x => x.Type == UserClaimTypes.IsGoogleLogin)
                .Value);

            httpContext.Items[RequestKeys.UserId] = user.Id;
            httpContext.Items[RequestKeys.UserEmail] = user.Email;
            httpContext.Items[RequestKeys.UserRole] = user.Role;
            httpContext.Items[RequestKeys.IsGoogleLogin] = isGoogleLogin.ToString();
        }
    }
}
