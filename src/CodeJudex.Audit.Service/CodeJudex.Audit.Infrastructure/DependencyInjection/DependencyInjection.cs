using CodeJudex.Audit.Application.Interfaces;
using CodeJudex.Audit.Domain.Rules;
using CodeJudex.Audit.Infrastructure.Authentication;
using CodeJudex.Audit.Infrastructure.Parsers;
using CodeJudex.Audit.Infrastructure.Rules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CodeJudex.Audit.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICodeParser, CSharpParser>();
        services.AddScoped<IAuditRule, NamingConventionRule>();
        services.AddScoped<IAuditRule, ComplexityRule>();
        services.AddScoped<IAuditRule, MagicNumberRule>();
        services.AddScoped<IAuditRule, StringBuilderRule>();

        var jwtSection = configuration.GetSection("JwtOptions");
        services.Configure<JwtOptions>(jwtSection);
        var jwtOptions = jwtSection.Get<JwtOptions>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        return services;
    }
}