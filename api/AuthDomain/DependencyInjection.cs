using api.AuthDomain.Options;
using api.AuthDomain.Services;

namespace api.AuthDomain;

public static class DependencyInjection
{
	public static IServiceCollection AddAuthDomain(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<JwtOption>(configuration.GetSection("Jwt"));

		services.AddScoped<AuthService>();
		return services;
	}
}
