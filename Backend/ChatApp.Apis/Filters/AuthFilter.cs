using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DataAccess;
using ChatApp.Entities.Models;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Apis.Filters
{
    public class AuthFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<AuthFilter> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthFilter(ILogger<AuthFilter> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(item => item is AllowAnonymousAttribute))
            {
                return;
            }

            try
            {
                var authToken = context.HttpContext.Request.Headers["Authorization"].ToString().Split();
                if (authToken.Length != 2 || authToken[0] != GlobalConstants.AuthSchema)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var token = authToken[1];
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration[AppSettingKeys.JwtSecret]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims
                    .First(x => FilterClaim(x, ClaimTypes.NameIdentifier))
                    .Value;

                var builder = MongoExtension.GetBuilders<User>();
                var user = await _unitOfWork
                    .GetRepository<User>()
                    .FirstOrDefault(builder.Eq(x => x.Id, userId));
                if (user == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                context.HttpContext.Items[RequestKeys.UserId] = user.Id;
                context.HttpContext.Items[RequestKeys.UserEmail] = user.Email;
                context.HttpContext.Items[RequestKeys.UserRole] = user.Role;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Auth filter error: {ex}");
                context.Result = new UnauthorizedResult();
            }
        }

        private static bool FilterClaim(Claim claim, string claimType)
        {
            var map = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap;

            if (map.TryGetValue(claim.Type, out var mapped))
            {
                return mapped == claimType;
            }

            return false;
        }
    }
}
