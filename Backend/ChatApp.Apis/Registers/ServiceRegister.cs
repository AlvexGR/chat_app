using ChatApp.DataAccess;
using ChatApp.Services.IServices;
using ChatApp.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Apis.Registers
{
    public static class ServiceRegister
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
