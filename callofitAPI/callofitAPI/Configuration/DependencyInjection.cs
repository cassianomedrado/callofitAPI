using callofitAPI.Interfaces;
using callofitAPI.Security.DAO;
using callofitAPI.Security.Service;
using netbullAPI.Middleware;
using netbullAPI.Security.MidwareDB;

namespace callofitAPI.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDIConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ChamadoMW>();
            services.AddScoped<HistoricoChamadoMW>();
            services.AddScoped<SistemaSuportadoMW>();
            services.AddScoped<StatusChamadoMW>();
            services.AddScoped<TipoChamadoMW>();
            services.AddScoped<TipoUsuarioMW>();
            services.AddScoped<UsuarioMW>();
            services.AddScoped<INotificador, NotificadorMW>();
            services.AddScoped<ChamadoDAO>();
            services.AddTransient<HistoricoChamadoDAO>();
            services.AddTransient<SistemaSuportadoDAO>();
            services.AddTransient<StatusChamadoDAO>();
            services.AddTransient<TipoChamadoDAO>();
            services.AddTransient<TipoUsuarioDAO>();
            services.AddTransient<UsuarioDAO>();
            services.AddTransient<TokenService>();

            return services;
        }
    }
}
