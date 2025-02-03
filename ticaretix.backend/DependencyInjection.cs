using ticaretix.Application;
using ticaretix.Core;
using ticaretix.Infrastructure;

namespace ticaretix.backend
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI()
                .AddInfrastructureDI();

            return services;
        }
    }
}
