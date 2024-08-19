using VotingSystem.Services;

namespace VotingSystem.Extensions;

public static class ConfigureAppDependenciesExtension
{
    public static void ConfigureAppDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IdentityService>();
    }
}