using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
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
        System.Console.WriteLine("---------------------------------------------------------------------");
        System.Console.WriteLine("Program to analyze Requirements and Test Cases from Office Documents.");
        System.Console.WriteLine("---------------------------------------------------------------------");
        System.Console.WriteLine("Possible arguments to command line:");
        System.Console.WriteLine(
            "--FileLocation:Path <Path> // Path to input and output files. Defaults to C:\\DATA\\V ");
        System.Console.WriteLine(
            "--Setting:RefreshTestCases true // Whether Test Cases should be read fresh from Office files. Defaults to false");
        System.Console.WriteLine(
            "--Setting:RefreshModuleReq true // Whether Module Requirements should be read fresh from Office files. Defaults to false");
        System.Console.WriteLine("");
        System.Console.WriteLine("---------------------------------------------------------------------");
        System.Console.WriteLine("Actual arguments: " + string.Join(' ', args));
        System.Console.WriteLine("---------------------------------------------------------------------");

        // Configure Serilog first
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        try
        {
            var host = CreateHostBuilder(args)
                .Build();

            var runtimeOptions = host
                .Services
                .GetRequiredService<IOptions<SettingOptions>>()
                .Value;

            if (runtimeOptions.RefreshTestCases)
                host
                    .Services
                    .GetRequiredService<ITestCaseRefresher>()
                    .Execute();

            if (runtimeOptions.RefreshModuleReq)
                host
                    .Services
                    .GetRequiredService<IModuleRequirementRefresher>()
                    .Execute();

            if (runtimeOptions.RefreshModuleReqTestCaseMapping)
                host
                    .Services
                    .GetRequiredService<IModuleReqTestCaseMappingRefresher>()
                    .Execute();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "During program.");
        }
        finally
        {
            Log.Logger.Information("Done.");
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
                    ["--Setting:RefreshTestCases"] = "Setting:RefreshTestCases",
                    ["--Setting:RefreshModuleReq"] = "Setting:RefreshModuleReq",
                    ["--Setting:RefreshModuleReqTestCaseMapping"] = "Setting:RefreshModuleReqTestCaseMapping",
                });
            })
            .UseSerilog()
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
                services.AddOptions<SettingOptions>()
                    .Bind(hostContext.Configuration.GetSection("Setting"));

                services.AddScoped<ITestCaseRefresher, TestCaseRefresher>();
                services.AddScoped<IModuleRequirementRefresher, ModuleRequirementRefresher>();
                services.AddScoped<IModuleReqTestCaseMappingRefresher, ModuleReqTestCaseMappingRefresher>();
            });
    }
}