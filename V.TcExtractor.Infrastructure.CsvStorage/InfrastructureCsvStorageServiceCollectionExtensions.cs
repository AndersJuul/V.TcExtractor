using Microsoft.Extensions.DependencyInjection;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage;

public static class InfrastructureCsvStorageServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureCsv(this IServiceCollection services)
    {
        services.AddScoped<ITestCaseRepository, TestCaseRepositoryCsv>();
        services.AddScoped<ITestResultRepository, TestResultRepositoryCsv>();
        services.AddScoped<IModuleRequirementRepository, ModuleRequirementRepositoryCsv>();
        services.AddScoped<IDvplItemRepository, DvplItemRepositoryCsv>();
        services.AddScoped<IBigJoinRepository, BigJoinRepositoryCsv>();
        services.AddScoped<IMatch1Repository, Match1RepositoryCsv>();

        return services;
    }
}