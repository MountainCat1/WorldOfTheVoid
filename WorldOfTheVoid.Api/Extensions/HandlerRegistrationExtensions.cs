using System.Reflection;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Extensions;

public static class HandlerRegistrationExtensions
{
    public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        var types = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .ToList();

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces()
                .Where(i =>
                    i.IsGenericType &&
                    (
                        i.GetGenericTypeDefinition().Name is nameof(IQueryHandler<,>) or nameof(ICommandHandler<,>)
                        // if ICommandHandler only has 1 type argument, handle that too
                        || (i.GetGenericTypeDefinition().Name == nameof(ICommandHandler<,>) && i.GetGenericArguments().Length == 1)
                    ));

            foreach (var handlerInterface in interfaces)
            {
                services.AddTransient(handlerInterface, type);
            }
        }

        return services;
    }

    public static IServiceCollection AddHandlersFromAssemblyContaining<T>(this IServiceCollection services)
        => services.AddHandlersFromAssembly(typeof(T).Assembly);
}