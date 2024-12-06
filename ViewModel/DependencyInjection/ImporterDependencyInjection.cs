using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services.Import;
using WpfApp1.ViewModel.Factories;

namespace WpfApp1.ViewModel.DependencyInjection
{
    internal static class ImporterDependencyInjection
    {
        public static void AddImporterFactory(this IServiceCollection services)
        {
            services.AddTransient<IDataImporter, CsvImporter>();
            services.AddTransient<IDataImporter, ExcelImporter>();
            services.AddTransient<IDataImporter, XmlImporter>();

            services.AddSingleton<Func<IEnumerable<IDataImporter>>>(x => () => x.GetService<IEnumerable<IDataImporter>>()!);

            services.AddSingleton<IImporterFactory, ImporterFactory>();
        }
    }
}
