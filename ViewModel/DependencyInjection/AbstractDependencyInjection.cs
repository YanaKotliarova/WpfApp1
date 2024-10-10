using Microsoft.Extensions.DependencyInjection;
using WpfApp1.ViewModel.Factories;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.DependencyInjection
{
    internal static class AbstractDependencyInjection
    {
        public static void AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddSingleton<TInterface, TImplementation>();
            services.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>()!);
            services.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
        }
    }
}
