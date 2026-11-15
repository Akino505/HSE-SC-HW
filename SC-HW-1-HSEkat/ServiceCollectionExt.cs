using SC_HW_1_HSEkat.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SC_HW_1_HSEkat
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddHSEKat(this IServiceCollection services)
        {
            services.AddSingleton<IServiceCenter, ServiceCenter>();
            services.AddSingleton<NumberGenerator>();
            services.AddSingleton<Park>();
            services.AddSingleton<ConsoleApp>();
            return services;
        }
    }

}
