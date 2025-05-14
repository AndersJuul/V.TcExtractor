using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace V.TcExtractor.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllImplementations<TInterface>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        params Assembly[] assemblies)
    {
        var interfaceType = typeof(TInterface);
        var assemblyList = assemblies.Any()
            ? assemblies
            : new[] { Assembly.GetExecutingAssembly(), interfaceType.Assembly };

        foreach (var assembly in assemblyList)
        {
            var implementations = assembly.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract);

            foreach (var implementation in implementations)
            {
                services.Add(new ServiceDescriptor(
                    interfaceType,
                    implementation,
                    lifetime));
            }
        }

        return services;
    }
}