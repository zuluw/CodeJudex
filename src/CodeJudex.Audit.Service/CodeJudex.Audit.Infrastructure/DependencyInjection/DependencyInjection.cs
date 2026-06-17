using CodeJudex.Audit.Application.Interfaces;
using CodeJudex.Audit.Domain.Rules;
using CodeJudex.Audit.Infrastructure.Parsers;
using CodeJudex.Audit.Infrastructure.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CodeJudex.Audit.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICodeParser, CSharpParser>();
        services.AddScoped<IAuditRule, NamingConventionRule>();
        services.AddScoped<IAuditRule, ComplexityRule>();

        return services;
    }
}