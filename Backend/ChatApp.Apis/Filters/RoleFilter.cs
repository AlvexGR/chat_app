using System;
using System.Linq;
using ChatApp.Utilities.Constants;
using ChatApp.Entities.Enums;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ChatApp.Apis.Filters
{
    public class RoleFilter : IActionFilter
    {
        private readonly ILogger<RoleFilter> _logger;
        private readonly UserRole[] _allowRoles;

        public RoleFilter(ILogger<RoleFilter> logger, UserRole[] allowRoles)
        {
            _logger = logger;
            _allowRoles = allowRoles;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var userRole = Convert
                    .ToInt32(context.HttpContext.Items[RequestKeys.UserRole]?.ToString())
                    .ToEnum<UserRole>();

                if (!_allowRoles.Contains(userRole))
                {
                    context.Result = new StatusCodeResult(403);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Role filter error: {ex}");
                context.Result = new StatusCodeResult(403);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
