using System;
using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Runtime.Registry
{
    public static class ServiceRegistry
    {
        private static readonly Dictionary<Type, object> services = new();

        public static void Register<T>(T? service) where T : class
        {
            if (service != null)
                services[typeof(T)] = service;
            else
                ForwardingLogger.LogException(new ArgumentNullException(nameof(service)));
        }

        /// <summary>
        /// Registers a service together with its configuration. Calls <c>SetConfiguration</c> on the service
        /// before storing it, so the service is fully configured from the start.
        /// </summary>
        public static void Register<TService, TConfig>(TService? service, TConfig? config)
            where TService : class, IService<TConfig>
            where TConfig : IServiceConfiguration
        {
            if (service == null)
            {
                ForwardingLogger.LogException(new ArgumentNullException(nameof(service)));
                return;
            }

            service.SetConfiguration(config);
            services[typeof(TService)] = service;
        }

        public static T Get<T>() where T : class
        {
            if (services.TryGetValue(typeof(T), out var service))
                return service as T;

            foreach (var (key, value) in services) //e.g. DefaultProcessRunner won't be found above, so we have to allow subtypes as well.
            {
                if (key.IsAssignableFrom(typeof(T)))
                    return value as T;
            }

            return null;
        }

        public static bool Has<T>() => services.Keys.Any(key => key.IsAssignableFrom(typeof(T)));
    }
}