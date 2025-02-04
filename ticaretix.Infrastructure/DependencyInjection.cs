using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ticaretix.Application.Interfaces;
using ticaretix.Application.Services;
using ticaretix.Application.UseCases;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;
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

            services.AddScoped<IUrunlerRepository, UrunlerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IKullaniciRepository, KullaniciRepository>();
            services.AddScoped<ISepetDetaylarıRepository, SepetDetayRepository>();
            services.AddScoped<ISepetRepository, SepetRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<LoginUseCase>();
            services.AddScoped<IRedisService, RedisService>();  // Redis servisini ekleyin
                                                                // Redis'i yapılandırma
            var redisConnectionString = configuration.GetValue<string>("Redis:ConnectionString");
            services.AddSingleton<IRedisService>(provider => new RedisService()); // Parametreyi doğru şekilde ilet


            return services;
        }
    }
}   
