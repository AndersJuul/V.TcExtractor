using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using V.TcExtractor.InputParsing;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;

namespace V.TcExtractor.Console;

public class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Get the App service and run it
        var folderScanner = host.Services.GetRequiredService<IFolderScanner>();
        var testCases = folderScanner.GetTestCases(args).ToArray();
        System.Console.WriteLine(testCases.Length);
        foreach (var testCase in testCases)
        {
            System.Console.WriteLine(testCase);
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IFolderScanner, FolderScanner>();
                services.AddAllImplementations<IFileProcessor>();
                services.AddAllImplementations<ITableAdapter>();
                services.AddAllImplementations<ICellAdapter>();
            });
}