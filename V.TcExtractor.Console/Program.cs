using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using V.TcExtractor.Application;
using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage;
using V.TcExtractor.Infrastructure.OfficeDocuments;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;

namespace V.TcExtractor.Console;

public class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args)
            .Build();

        var runtimeOptions = host.Services.GetRequiredService<IOptions<InputRefreshOptions>>().Value;

        if (runtimeOptions.ShouldRefreshTestCases)
            host
                .Services
                .GetRequiredService<ITestCaseRefresher>()
                .Execute();

        if (runtimeOptions.ShouldRefreshModuleReq)
            host
                .Services
                .GetRequiredService<IModuleRequirementRefresher>()
                .Execute();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddCommandLine(args, new Dictionary<string, string>
                {
                    ["--FileLocation:Path"] = "FileLocation:Path",
                    ["--InputRefresh:ShouldRefreshTestCases"] = "InputRefresh:ShouldRefreshTestCases",
                    ["--InputRefresh:ShouldRefreshModuleReq"] = "InputRefresh:ShouldRefreshModuleReq",
                });
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IFolderScanner, FolderScanner>();
                services.AddScoped<ITestCaseRequirementMatcher, TestCaseRequirementMatcher>();
                services.AddAllImplementations<ITestCaseFileProcessor>();
                services.AddAllImplementations<IModuleRequirementFileProcessor>();
                services.AddAllImplementations<ITableAdapter>();
                services.AddAllImplementations<ICellAdapter>();

                services.AddScoped<ITestCaseRepository, TestCaseRepositoryCsv>();
                services.AddScoped<IModuleRequirementRepository, ModuleRequirementRepositoryCsv>();

                services.AddOptions<FileLocationOptions>()
                    .Bind(hostContext.Configuration.GetSection("FileLocation"))
                    .Validate(options => !string.IsNullOrEmpty(options.Path), "Path is required");
                services.AddOptions<InputRefreshOptions>()
                    .Bind(hostContext.Configuration.GetSection("InputRefresh"));

                services.AddScoped<ITestCaseRefresher, TestCaseRefresher>();
                services.AddScoped<IModuleRequirementRefresher, ModuleRequirementRefresher>();
            });
    }
}