using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ticaretix.Infrastructure.Logging
{
    public static class LoggingService
    {
        public static void ConfigureLogging(IServiceCollection services, IConfiguration configuration)
        {
            // Serilog yapılandırmasını oluştur
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)  // appsettings.json'dan okuma
                .WriteTo.Console()
                .WriteTo.File("logs/app.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Serilog'u DI Container'a ekle
            services.AddSingleton<ILogger>((ILogger)Log.Logger);
        }
    }
}
