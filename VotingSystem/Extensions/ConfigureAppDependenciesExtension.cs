using VotingSystem.Repositories;
using VotingSystem.Services;

namespace VotingSystem.Extensions;

public static class ConfigureAppDependenciesExtension
{
    public static void ConfigureAppDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IdentityService>();
        builder.Services.AddTransient<UserRepository>();
        builder.Services.AddTransient<VotingService>();
    }
}