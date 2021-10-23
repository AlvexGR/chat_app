using ChatApp.SignalR.Hubs;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.SignalR.Registers
{
    public static class HubRegister
    {
        public static void AddHubs(this IServiceCollection services)
        {
            services.AddScoped<ChatHub>();
        }
    }
}
