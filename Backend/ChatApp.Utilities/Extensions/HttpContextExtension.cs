﻿using System;
using ChatApp.Entities.Enums;
using ChatApp.Utilities.Constants;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Utilities.Extensions
{
    public static class HttpContextExtension
    {
        public static string UserId(this HttpContext httpContext)
        {
            var canParse = httpContext.Items.TryGetValue(RequestKeys.UserId, out var userId);
            return canParse ? userId.ToString() : default;
        }

        public static string UserEmail(this HttpContext httpContext)
        {
            var canParse = httpContext.Items.TryGetValue(RequestKeys.UserEmail, out var userEmail);
            return canParse ? userEmail.ToString() : default;
        }

        public static UserRole UserRole(this HttpContext httpContext)
        {
            var canParse = httpContext.Items.TryGetValue(RequestKeys.UserRole, out var userRole);
            return canParse ? Convert.ToInt32(userRole.ToString()).ToEnum<UserRole>() : default;
        }

        public static bool IsGoogleLogin(this HttpContext httpContext)
        {
            var canParse = httpContext.Items.TryGetValue(RequestKeys.IsGoogleLogin, out var isGoogleLogin);
            return canParse ? Convert.ToBoolean(isGoogleLogin.ToString()) : default;
        }
    }
}
