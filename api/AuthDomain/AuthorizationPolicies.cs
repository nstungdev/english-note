namespace api.AuthDomain
{
    public static class AuthorizationPolicies
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(Constants.UserManagerPolicy, policy =>
                    policy.RequireClaim(
                        Constants.PermissionsClaimType,
                        "system:create",
                        "system:read",
                        "system:update",
                        "system:delete"));

            return services;
        }
    }
}