using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Apis.Hubs
{
    public class BaseHub : Hub
    {
        private readonly IAuthService _authService;
        protected static readonly ConcurrentDictionary<string, List<string>> Connections = new();

        public BaseHub(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var (token, user) = await _authService
                .VerifyAuthToken(httpContext.Request
                    .Query[RequestKeys.AccessToken].ToString());

            var isGoogleLogin = Convert.ToBoolean(token.Claims
                .First(x => x.Type == UserClaimTypes.IsGoogleLogin)
                .Value);

            httpContext.Items[RequestKeys.UserId] = user.Id;
            httpContext.Items[RequestKeys.UserEmail] = user.Email;
            httpContext.Items[RequestKeys.UserRole] = user.Role;
            httpContext.Items[RequestKeys.IsGoogleLogin] = isGoogleLogin.ToString();

            var canGet = Connections.TryGetValue(user.Id, out var connections);
            if (!canGet)
            {
                Connections.TryAdd(user.Id, new List<string> { Context.ConnectionId });
                return;
            }

            connections.Add(Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.UserId();
            var canGet = Connections.TryGetValue(userId, out var connections);
            if (!canGet || !connections.Contains(Context.ConnectionId)) return Task.CompletedTask;

            connections.Remove(Context.ConnectionId);
            if (connections.Any()) return Task.CompletedTask;

            Connections.Remove(Context.ConnectionId, out _);
            return Task.CompletedTask;
        }
    }
}
