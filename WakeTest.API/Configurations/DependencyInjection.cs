using WakeTest.Application.Interfaces;
using WakeTest.Application.Services;

namespace WakeTest.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();


            return services;
        }
    }
}
