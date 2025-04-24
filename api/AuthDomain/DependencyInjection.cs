using api.AuthDomain.Options;

namespace api.AuthDomain;

public static class DependencyInjection
{
	public static IServiceCollection AddAuthDomain(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<JwtOption>(configuration.GetSection("Jwt"));


		return services;
	}
}
