using CodeJudex.Audit.Application.Interfaces;
using CodeJudex.Audit.Application.Mappings;
using CodeJudex.Audit.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CodeJudex.Audit.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuditService, AuditService>();
        services.AddSingleton<AuditMapper>();
        services.AddValidatorsFromAssemblyContaining<AuditService>();

        return services;
    }
}