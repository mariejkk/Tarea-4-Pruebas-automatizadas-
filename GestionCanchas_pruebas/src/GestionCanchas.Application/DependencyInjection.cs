using GestionCanchas.Application.Interfaces;
using GestionCanchas.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GestionCanchas.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICanchaService, CanchaService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
