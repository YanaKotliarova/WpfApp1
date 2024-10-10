using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Model.Export;
using WpfApp1.ViewModel.Factories;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.DependencyInjection
{
    internal static class ExporterDependencyInjection
    {
        public static void AddExporterFactory(this IServiceCollection services)
        {
            services.AddTransient<IDataExporter, ExcelExporter>();
            services.AddTransient<IDataExporter, XmlExporter>();

            services.AddSingleton<Func<IEnumerable<IDataExporter>>>(x => () => x.GetService<IEnumerable<IDataExporter>>()!);

            services.AddSingleton<IExporterFactory, ExporterFactory>();
        }
    }
}
