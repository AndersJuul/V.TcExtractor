using Microsoft.Extensions.DependencyInjection;
using V.TcExtractor.Console;
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

            services.AddAllImplementations<ITestCaseFileProcessor>();
            services.AddAllImplementations<IModuleRequirementFileProcessor>();
            services.AddAllImplementations<IDvplFileProcessor>();
            services.AddAllImplementations<ITableAdapter>();
            services.AddAllImplementations<ICellAdapter>();

            return services;
        }
    }
}