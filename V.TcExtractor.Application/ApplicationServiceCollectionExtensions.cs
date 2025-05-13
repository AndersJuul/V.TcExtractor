using Microsoft.Extensions.DependencyInjection;
using V.TcExtractor.Domain.Refreshers;

namespace V.TcExtractor.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<ITestCaseRefresher, TestCaseRefresher>();
            services.AddScoped<IModuleRequirementRefresher, ModuleRequirementRefresher>();
            services.AddScoped<IDVPLRefresher, DVPLRefresher>();
            services.AddScoped<IModuleReqTestCaseMappingRefresher, ModuleReqTestCaseMappingRefresher>();
            services.AddScoped<IBigJoinRefresher, BigJoinRefresher>();
            services.AddScoped<ITestResultRefresher, TestResultRefresher>();

            return services;
        }
    }
}