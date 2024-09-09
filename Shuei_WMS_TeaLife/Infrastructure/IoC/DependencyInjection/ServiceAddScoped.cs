using Application.Services;
using Application.Services.Authen;
using Infrastructure.Repos;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.IoC.DependencyInjection
{
    public static class ServiceAddScoped
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAccount, RepositoryAccountServices>();
            services.AddScoped<IProduct, RepositoryProductServices>();
            services.AddScoped<IUnit, RepositoryUnitServices>();

            services.AddScoped<Repository>();
        }
    }
}
