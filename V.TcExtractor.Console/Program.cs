using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using V.TcExtractor.InputParsing;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
using V.TcExtractor.OutputFormatting;

namespace V.TcExtractor.Console;

public class Program
{
    static void Main(string[] args)
    {
        // Build IoC container and configuration
        var host = CreateHostBuilder(args).Build();
        var config = CreateConfigurationBuilder(args)
            .Build();

        var pathToFiles = args.FirstOrDefault() ?? "c:\\data\\dvpr";
        var outputFormatter = (config["output"] ?? "console").ToLower();

        // Get classes required for processing
        var folderScanner = host.Services.GetRequiredService<IFolderScanner>();
        var output = host.Services.GetServices<ITestCaseOutput>()
            .SingleOrDefault(x => x.CanHandle(outputFormatter));
        if (output == null)
        {
            System.Console.WriteLine($"Output formatter '{outputFormatter}' not found.");
            return;
        }

        // Get test cases from the folder and write them to the output
        var testCases = folderScanner.GetTestCases(pathToFiles).ToArray();
        output.Write(testCases);
    }

    private static IConfigurationBuilder CreateConfigurationBuilder(string[] args)
    {
        return new ConfigurationBuilder()
            .AddCommandLine(args);
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IFolderScanner, FolderScanner>();
                services.AddAllImplementations<IFileProcessor>();
                services.AddAllImplementations<ITableAdapter>();
                services.AddAllImplementations<ICellAdapter>();
                services.AddAllImplementations<ITestCaseOutput>();
            });
}