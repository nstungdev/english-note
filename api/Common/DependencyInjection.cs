using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using api.Common.Managers;

namespace api.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32))),
                ServiceLifetime.Scoped);

            // Register other services here if needed
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthManager, AuthManager>();

            return services;
        }
    }
}