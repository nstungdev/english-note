using Microsoft.Extensions.DependencyInjection;
using Common.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthDomain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthDomain(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32))));

            // Register other services here if needed

            return services;
        }
    }
}