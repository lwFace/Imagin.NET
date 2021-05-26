using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// Stores a single instance of any <see cref="Type"/> indefinitely. 
    /// Use <see cref="New(Type, object)"/> or <see cref="New{T}(object)"/>; subsequent duplicates are ignored. 
    /// Use <see cref="Current{T}"/> and <see cref="Where{T}"/> to access globally.
    /// </summary>
    public static class Get
    {
        static Dictionary<Type, object> instances = new Dictionary<Type, object>();

        /// <summary>
        /// Specify the actual <see cref="Type"/>, not an inherited <see cref="Type"/>; see <see cref="Where{T}"/> for accessing by inherited <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        public static void New(Type type, object instance)
        {
            if (instances.ContainsKey(type))
            {
                instances[type] = instance;
                return;
            }
            instances.Add(type, instance);
        }

        public static void New<T>(object instance) => New(typeof(T), instance);

        public static T Current<T>()
        {
            if (instances.ContainsKey(typeof(T)))
                return (T)instances[typeof(T)];

            return default(T);
        }

        public static T Where<T>() where T : class
        {
            foreach (var i in instances)
            {
                if (i.Key.Inherits(typeof(T), true) || (typeof(T).IsInterface && i.Key.Implements<T>()))
                    return (T)i.Value;
            }

            return default(T);
        }

        public static bool Exists<T>() => Current<T>() != null;
    }
}