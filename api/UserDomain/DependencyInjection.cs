using System;
using api.UserDomain.Services;

namespace api.UserDomain;

public static class DependencyInjection
{
    public static IServiceCollection AddUserDomain(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        return services;
    }
}
