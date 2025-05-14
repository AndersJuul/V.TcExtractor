using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using V.TcExtractor.Application;
using V.TcExtractor.Domain;
using V.TcExtractor.Infrastructure.CsvStorage;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Integration.Tests
{
    public class ServiceRegistrationIntegrationTests
    {
        [Fact]
        public void AllConcreteServicesCanBeResolved()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["FileLocation:Path"] = Path.GetTempPath(),
                })
                .Build();
            services.AddDomainLayer(configuration);
            services.AddApplicationLayer();
            services.AddInfrastructureOfficeDocuments();
            services.AddInfrastructureCsv();
            services.AddLogging();

            var provider = services.BuildServiceProvider();

            var assemblies = new Assembly[]
            {
                typeof(ApplicationServiceCollectionExtensions).Assembly,
                typeof(DomainServiceCollectionExtensions).Assembly,
                typeof(InfrastructureCsvStorageServiceCollectionExtensions).Assembly,
                typeof(InfrastructureOfficeDocumentsServiceCollectionExtensions).Assembly,
            };

            var failures = new List<string>();

            foreach (var type in assemblies
                         .SelectMany(a => a.GetTypes())
                         .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic))
            {
                try
                {
                    // Try to create instance from provider
                    var instance = ActivatorUtilities.CreateInstance(provider, type);
                }
                catch (Exception ex)
                {
                    failures.Add($"{type.FullName}: {ex.Message}");
                }
            }

            Assert.True(failures.Count == 0, $"Unresolvable services:\n{string.Join("\n", failures)}");
        }

        [Fact]
        public void AllInterfacesMustHaveAtLeastOneImplementation()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["FileLocation:Path"] = Path.GetTempPath(),
                })
                .Build();
            services.AddDomainLayer(configuration);
            services.AddApplicationLayer();
            services.AddInfrastructureOfficeDocuments();
            services.AddInfrastructureCsv();
            services.AddLogging();

            var provider = services.BuildServiceProvider();

            var assemblies = new Assembly[]
            {
                typeof(DomainServiceCollectionExtensions).Assembly,
                typeof(ApplicationServiceCollectionExtensions).Assembly,
                typeof(InfrastructureOfficeDocumentsServiceCollectionExtensions).Assembly,
                typeof(InfrastructureCsvStorageServiceCollectionExtensions).Assembly,
            };

            var failures = new List<string>();

            foreach (var type in assemblies
                         .SelectMany(a => a.GetTypes())
                         .Where(t => t.IsInterface && t.IsPublic))
            {
                try
                {
                    // Try to create instance from provider
                    var implementations = provider.GetRequiredService(type);
                }
                catch (Exception ex)
                {
                    failures.Add($"{type.FullName}: {ex.Message}");
                }
            }

            Assert.True(failures.Count == 0, $"Unresolvable services:\n{string.Join("\n", failures)}");
        }
    }
}