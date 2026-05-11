using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Mappings;
using TaskFlow.Application.Services;

namespace TaskFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddScoped<IProjectService, ProjectService>();

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}