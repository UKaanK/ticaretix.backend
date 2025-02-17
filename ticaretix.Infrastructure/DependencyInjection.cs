using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ticaretix.Application.Interfaces;
using ticaretix.Application.Services;
using ticaretix.Application.UseCases;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;
using ticaretix.Infrastructure.Logging;
using ticaretix.Infrastructure.Middlewares;
using ticaretix.Infrastructure.Redis;
using ticaretix.Infrastructure.Repositories;

namespace ticaretix.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the JwtSettings section
            var jwtSettingsSection = configuration.GetSection("JwtSettings");

            // Create a JwtSettings object and populate it from the section
            var jwtSettings = new JwtSettings();
            jwtSettingsSection.Bind(jwtSettings);

            // Configure using the populated object
            services.Configure<JwtSettings>(options =>
            {
                options.Secret = jwtSettings.Secret;
                options.Issuer = jwtSettings.Issuer;
                options.Audience = jwtSettings.Audience;
                options.ExpiryMinutes = jwtSettings.ExpiryMinutes; // If you have this property
            });
            // Bağlantı stringini burada doğrudan tanımla
            const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ticaretix;Trusted_Connection=True;";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<IUrunlerRepository, UrunlerRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IKullaniciRepository, KullaniciRepository>();
            services.AddSingleton<ISepetDetaylarıRepository, SepetDetayRepository>();
            services.AddSingleton<ISepetRepository, SepetRepository>();
            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<LoginUseCase>();
            services.AddSingleton<RefreshTokenUseCase>();
            //services.AddTransient<ExceptionMiddleware>(); // Transient olarak kaydedin

             services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
            services.AddSingleton<IJwtDecoderService, JwtDecoderService>();


            services.AddSingleton<IRedisService, RedisService>();  // Redis servisini ekleyin
                                                                // Redis'i yapılandırma


            return services;
        }
    }
}   
