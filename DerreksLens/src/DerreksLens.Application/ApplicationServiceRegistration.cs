using DerreksLens.Application.Interfaces.Services;
using DerreksLens.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DerreksLens.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}