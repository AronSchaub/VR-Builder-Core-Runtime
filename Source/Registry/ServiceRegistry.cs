using System;
using System.Collections.Generic;

namespace VRBuilder.Core.Runtime.Registry
{
    public static class ServiceRegistry
    {
        private static readonly Dictionary<Type, object> services = new();

        public static void Register<T>(T service) where T : class
        {
            services[typeof(T)] = service ?? throw new ArgumentNullException(nameof(service));
        }

        public static T Get<T>() where T : class
        {
            if (services.TryGetValue(typeof(T), out var service))
                return service as T;

            foreach (var (key, value) in services) //e.g. DefaultProcessRunner won't be found above, so we have to allow subtypes as well.
            {
                if (typeof(T).IsAssignableFrom(key))
                    return value as T;
            }

            return null;
        }

        public static bool Has<T>()
        {
            return services.ContainsKey(typeof(T));
        }
    }
}