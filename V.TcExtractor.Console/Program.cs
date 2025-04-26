using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using V.TcExtractor.InputParsing;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;

namespace V.TcExtractor.Console;

public class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Get the App service and run it
        var folderScanner = host.Services.GetRequiredService<IFolderScanner>();
        folderScanner.Scan(args);
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IFolderScanner, FolderScanner>();
                services.AddAllImplementations<IFileProcessor>();
            });
}