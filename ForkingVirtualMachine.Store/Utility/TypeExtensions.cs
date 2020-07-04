namespace ForkingVirtualMachine.Store.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        /// <summary>
        /// Not abstract, not an interface, not generic type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this Type t)
        {
            return !t.IsAbstract && !t.IsInterface && !t.IsGenericType;
        }

        /// <summary>
        /// Can pull all implemented types like typeof(IRequestHandler{,}) from the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementedGenericInterfaces(this Type type, Type genericType)
        {
            return type
                .GetTypeInfo()
                .ImplementedInterfaces
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
        }

        public static bool Implements<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
