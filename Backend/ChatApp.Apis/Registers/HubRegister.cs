using ChatApp.Apis.Hubs.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Apis.Registers
{
    public static class HubRegister
    {
        public static void AddHubs(this IServiceCollection services)
        {
            services.AddScoped<ChatHub>();
        }

        public static void MapSignalRHubs(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<ChatHub>("/v1/chat");
        }
    }
}
