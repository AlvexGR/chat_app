using ChatApp.Apis.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Apis.Registers
{
    public static class FilterRegister
    {
        public static void AddFilters(this IServiceCollection services)
        {
            services.AddScoped<AuthFilter>();
        }
    }
}
