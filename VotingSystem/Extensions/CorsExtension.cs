namespace VotingSystem.Extensions;

public static class CorsExtension
{
    public static void ConfigureAppCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(policyBuilder =>
            policyBuilder.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://localhost:8080") // Especifique a origem permitida aqui
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials() // Permite que as credenciais sejam incluídas
            ));
    }
}