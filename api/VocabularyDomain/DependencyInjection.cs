using api.VocabularyDomain.Services;

namespace api.VocabularyDomain;

public static class DependencyInjection
{
    public static IServiceCollection AddVocabularyDomain(this IServiceCollection services)
    {
        services.AddScoped<VocabularyService>();
        return services;
    }
}