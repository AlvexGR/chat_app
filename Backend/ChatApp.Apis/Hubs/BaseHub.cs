using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Apis.Hubs
{
    public class BaseHub : Hub
    {
        protected static readonly ConcurrentDictionary<string, List<string>> Connections = new();

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.UserId();
            var canGet = Connections.TryGetValue(userId, out var connections);
            if (!canGet)
            {
                Connections.TryAdd(userId, new List<string> { Context.ConnectionId });
                return Task.CompletedTask;
            }

            connections.Add(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.UserId();
            var canGet = Connections.TryGetValue(userId, out var connections);
            if (!canGet) return Task.CompletedTask;

            var connectionIdx = connections.IndexOf(Context.ConnectionId);
            if (connectionIdx != -1) connections.RemoveAt(connectionIdx);
            if (connections.Any()) return Task.CompletedTask;

            Connections.Remove(Context.ConnectionId, out _);
            return Task.CompletedTask;
        }

        protected IReadOnlyCollection<string> GetOnlineUsers(params string[] userIds)
        {
            var result = new List<string>();
            foreach (var userId in userIds)
            {
                var canGet = Connections.TryGetValue(userId, out var connectionIds);
                if (!canGet) continue;

                result.AddRange(connectionIds);
            }

            return result;
        }
    }
}
