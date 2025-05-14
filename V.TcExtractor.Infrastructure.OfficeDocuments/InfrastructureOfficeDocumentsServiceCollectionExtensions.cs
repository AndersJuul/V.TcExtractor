using Microsoft.Extensions.DependencyInjection;
using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments
{
    public static class InfrastructureOfficeDocumentsServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureOfficeDocuments(this IServiceCollection services)
        {
            services.AddScoped<IFolderScanner, FolderScanner>();

            var serviceLifetime = ServiceLifetime.Scoped;
            var assemblies = new[] { typeof(TestCaseFileProcessor).Assembly };

            services.AddAllImplementations<ITestCaseFileProcessor>(serviceLifetime, assemblies);
            services.AddAllImplementations<ITestResultFileProcessor>(serviceLifetime, assemblies);
            services.AddAllImplementations<IModuleRequirementFileProcessor>(serviceLifetime, assemblies);
            services.AddAllImplementations<IDvplFileProcessor>(serviceLifetime, assemblies);
            services.AddAllImplementations<ITableAdapter>();
            services.AddAllImplementations<ICellAdapter>();

            return services;
        }
    }
}