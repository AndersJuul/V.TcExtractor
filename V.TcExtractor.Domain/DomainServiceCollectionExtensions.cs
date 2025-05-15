using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using V.TcExtractor.Domain.Adapters;
using V.TcExtractor.Domain.Options;

namespace V.TcExtractor.Domain
{
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ITestCaseRequirementMatcher, TestCaseRequirementMatcher>();
            services.AddScoped<IPassedTextAdapter, PassedTextAdapter>();

            services.AddOptions<FileLocationOptions>()
                .Bind(configuration.GetSection("FileLocation"))
                .Validate(options => !string.IsNullOrEmpty(options.Path), "Path is required");
            services.AddOptions<SettingOptions>()
                .Bind(configuration.GetSection("Setting"));
            return services;
        }
    }
}