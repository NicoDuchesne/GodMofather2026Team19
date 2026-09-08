using System;
using System.Linq;

namespace BitDuc.Support
{
    public static class TypeImplementationsExtension
    {
        public static Type[] Implementations(this Type type) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type.IsAssignableFrom)
                .Where(assignable => !assignable.IsInterface && !assignable.IsAbstract)
                .ToArray();
    }
}