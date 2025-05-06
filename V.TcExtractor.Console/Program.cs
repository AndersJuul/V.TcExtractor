using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using V.TcExtractor.Application;
using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Options;
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
        // Build IoC container and configuration
        var host = CreateHostBuilder(args)
            .Build();

        var runtimeOptions = host.Services.GetRequiredService<IOptions<InputRefreshOptions>>().Value;

        if (runtimeOptions.ShouldRefreshTestCases)
        {
            // Resolve the IUpdateTc service and execute it
            host
                .Services
                .GetRequiredService<IUpdateTc>()
                .Execute();
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddCommandLine(args, new Dictionary<string, string>
                {
                    ["--FileLocation:Path"] = "FileLocation:Path",
                    ["--InputRefresh:ShouldRefreshTestCases"] =
                        "InputRefresh:ShouldRefreshTestCases", // Add this mapping
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

                //--FileLocation:Path "C:\Data\V" --output csv --InputRefresh:Execute yes
                services.AddOptions<FileLocationOptions>()
                    .Bind(hostContext.Configuration.GetSection("FileLocation"))
                    .Validate(options => !string.IsNullOrEmpty(options.Path), "Path is required");
                services.AddOptions<InputRefreshOptions>()
                    .Bind(hostContext.Configuration.GetSection("InputRefresh"));

                services.AddScoped<IUpdateTc, UpdateTc>();
            });
    }
}