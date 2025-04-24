using Microsoft.Extensions.DependencyInjection;
using Common.Data;
using Microsoft.EntityFrameworkCore;

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
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32))));

            // Register other services here if needed

            return services;
        }
    }
}