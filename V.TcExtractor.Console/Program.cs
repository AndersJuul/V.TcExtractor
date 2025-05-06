using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using V.TcExtractor.InputParsing;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
using V.TcExtractor.Model;
using V.TcExtractor.OutputFormatting;

namespace V.TcExtractor.Console;

public class Program
{
    static void Main(string[] args)
    {
        // Build IoC container and configuration
        var config = CreateConfigurationBuilder(args)
            .Build();
        var host = CreateHostBuilder(args, config).Build();

        config["pathToFiles"] = args.FirstOrDefault() ?? "c:\\data\\v";
        var outputFormatter = (config["output"] ?? "console").ToLower();

        System.Console.WriteLine(
            $"Trying to read files from {config["pathToFiles"]} and send out to {outputFormatter}.");
        System.Console.WriteLine("First arg to application is the path, defaulting to c:\\data\\v ");
        System.Console.WriteLine("Use --output (csv|console) to write to console or csv files, defaulting to console");

        // Get classes required for processing
        var folderScanner = host.Services.GetRequiredService<IFolderScanner>();
        var output = host.Services.GetServices<ITestCaseOutput>()
            .SingleOrDefault(x => x.CanHandle(outputFormatter));
        if (output == null)
        {
            System.Console.WriteLine($"Output formatter '{outputFormatter}' not found.");
            return;
        }

        // Get test cases, module requirements from the folder and write them to the output
        var testCases = folderScanner
            .GetTestCases()
            .ToArray();
        var moduleRequirements = folderScanner
            .GetModuleRequirements()
            .ToArray();

        System.Console.WriteLine($"Read from files: {testCases.Length} TCs and {moduleRequirements.Length} MRs");

        var matcher = host.Services.GetRequiredService<ITestCaseRequirementMatcher>();
        foreach (var moduleRequirement in moduleRequirements)
        {
            var matchingTestCases = testCases.Where(x => matcher.IsMatch(moduleRequirement, x)).ToArray();
            if (!matchingTestCases.Any())
                continue;
            //var match = new Match(moduleRequirement, matchingTestCases);
        }

        output.Write(testCases);
        output.Write(moduleRequirements);
    }

    private static IConfigurationBuilder CreateConfigurationBuilder(string[] args)
    {
        return new ConfigurationBuilder()
            .AddCommandLine(args);
    }

    static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot config) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped(c => new OutputFolder(config["pathToFiles"]!));
                services.AddScoped(c => new InputFolder(config["pathToFiles"]!));
                services.AddScoped<IFolderScanner, FolderScanner>();
                services.AddScoped<ITestCaseRequirementMatcher, TestCaseRequirementMatcher>();
                services.AddAllImplementations<ITestCaseFileProcessor>();
                services.AddAllImplementations<IModuleRequirementFileProcessor>();
                services.AddAllImplementations<ITableAdapter>();
                services.AddAllImplementations<ICellAdapter>();
                services.AddAllImplementations<ITestCaseOutput>();
            });
}

public interface ITestCaseRequirementMatcher
{
    bool IsMatch(ModuleRequirement moduleRequirement, TestCase testCase);
}

public class TestCaseRequirementMatcher : ITestCaseRequirementMatcher
{
    public bool IsMatch(ModuleRequirement moduleRequirement, TestCase testCase)
    {
        throw new NotImplementedException();
    }
}