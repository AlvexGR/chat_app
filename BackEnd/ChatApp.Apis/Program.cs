using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

namespace ChatApp.Apis
{
    public class Program
    {
        private static readonly LoggerProviderCollection Providers = new LoggerProviderCollection();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, services, loggerConfiguration) =>
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext()
                            .WriteTo.Providers(Providers)
                            .WriteTo.File(
                                $"Logs/{DateTime.Now.Date:dd-MM-yyyy}/ChatApp.txt",
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{RequestId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                fileSizeLimitBytes: 50000000,
                                rollOnFileSizeLimit: true),
                    writeToProviders: true)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
