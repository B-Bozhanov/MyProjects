namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    using DependencyInjection;

    public static class WebApplicationBuilderExtencions
    {
        public static void AddServices(this IServiceCollection services, Type[] serviceTypes)
        {
            foreach (var type in serviceTypes)
            {
                var serviceAssembly = Assembly.GetAssembly(type);

                if (serviceAssembly == null)
                {
                    throw new InvalidOperationException("Invalid service type!");
                }

                var currentServiceTypes = serviceAssembly
                     .GetTypes()
                     .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                     .ToArray();

                foreach (var implementationType in currentServiceTypes)
                {
                    //if (implementationType.Name == "SupportLanguageService")
                    //{
                    //    continue;
                    //}
                    var interfaceType = implementationType.GetInterface($"I{implementationType.Name}");

                    if (interfaceType == null)
                    {
                        throw new InvalidOperationException("No interface is Provided");
                    }

                    services.AddScoped(interfaceType, implementationType);
                }
            }
        }
    }
}
