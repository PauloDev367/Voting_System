using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VotingSystem.Configurations;
using VotingSystem.Data;
using VotingSystem.Entities;

namespace VotingSystem.Extensions;

public static class ConfigureIdentityExtension
{
    public static void ConfigureIdentity(this WebApplicationBuilder builder)
    {
        var securityKey =
            new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtOptions:SecurityKey").Value));
        var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtOptions));

        ConfigureJwtToken(builder.Services, builder.Configuration, securityKey, jwtAppSettingOptions);
        ConfigureIdentityUser(builder.Services);
        ConfigurePasswordRequirements(builder.Services);
    }

    private static void ConfigurePasswordRequirements(IServiceCollection service)
    {
        service.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
        });
    }

    private static void ConfigureIdentityUser(IServiceCollection service)
    {
        service.AddDefaultIdentity<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureJwtToken(IServiceCollection service, IConfiguration configuration,
        SymmetricSecurityKey securityKey, IConfigurationSection jwtAppSettingOptions)
    {
        service.Configure<JwtOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
            options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
            options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            options.Expiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.Expiration)] ?? "0");
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,

            ValidateAudience = true,
            ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,

            ClockSkew = TimeSpan.Zero
        };

        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // Se a solicitação for para o hub do SignalR
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/votes")))
                    {
                        // Token JWT será recuperado de um query string
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }
}