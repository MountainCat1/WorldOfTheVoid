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

        var handlerInterfaceTypes = new[]
        {
            typeof(IQueryHandler<,>),
            typeof(ICommandHandler<,>)
        };

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            handlerInterfaceTypes.Contains(i.GetGenericTypeDefinition()));

            if(interfaces.Any() == false)
                continue;
            
            services.AddTransient(type, type);

            Console.WriteLine($"Registered handler: {type.FullName}");
        }


        return services;
    }

    public static IServiceCollection AddHandlersFromAssemblyContaining<T>(this IServiceCollection services)
        => services.AddHandlersFromAssembly(typeof(T).Assembly);
}