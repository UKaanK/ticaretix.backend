using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;
using ticaretix.Infrastructure.Repositories;

namespace ticaretix.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            // Bağlantı stringini burada doğrudan tanımla
            const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ticaretix;Trusted_Connection=True;";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUrunlerRepository, UrunlerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IKullaniciRepository, KullaniciRepository>();
            services.AddScoped<ISepetDetaylarıRepository, SepetDetayRepository>();
            services.AddScoped<ISepetRepository, SepetRepository>();

            return services;
        }
    }
}   
