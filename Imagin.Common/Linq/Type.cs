using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Create a new instance of the given type using <see cref="Activator.CreateInstance()"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Create<T>(this Type input, params object[] parameters) => (T)Activator.CreateInstance(input, parameters);

        public static bool Equals<Type>(this System.Type input) => input == typeof(Type);

        public static ObservableCollection<Enum> Enumerate(this Type input, Appearance appearance, bool sort = false)
        {
            if (!input.IsEnum)
                throw new ArgumentOutOfRangeException(nameof(input));

            if (appearance == Appearance.None)
                throw new NotSupportedException();

            var result = new ObservableCollection<Enum>();

            var values = input.GetEnumValues().Cast<object>();
            values = sort ? values.OrderBy(i => i.ToString()) : values;
            foreach (var i in values)
            {
                var field = (Enum)i;
                switch (appearance)
                {
                    case Appearance.Hidden:
                        continue;

                    case Appearance.Visible:
                    case Appearance.All:
                        result.Add(field);
                        break;
                }
            }
            return result;
        }

        public static IEnumerable<Type> GetTypes(this Type input, string @namespace)
        {
            var result = from type in Assembly.GetCallingAssembly().GetTypes()
            where
            (
                type.IsClass
                &&
                type.Namespace == @namespace
                &&
                type.Inherits(input, true)
                &&
                !type.IsAbstract
                &&
                type.GetAttribute<HiddenAttribute>() == null
            )
            select type;
            foreach (var i in result)
                yield return i;
        }

        /// <summary>
        /// Gets whether or not a type implements the interface <see cref="{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Implements<T>(this Type input) where T : class
        {
            if (!typeof(T).GetTypeInfo().IsInterface)
                throw new InvalidCastException("Type is not an interface.");

            return typeof(T).IsAssignableFrom(input);
        }

        public static bool Inherits<T>(this Type input, bool include = false) => input.Inherits(typeof(T), include);

        public static bool Inherits(this Type a, Type b, bool include = false)
        {
            if (!include)
            {
                if (a.Equals(b))
                    return false;
            }

            while (!a.Equals(typeof(object)))
            {
                if (a.Equals(b))
                    return true;

                a = a.BaseType;
            }
            return false;
        }

        public static IEnumerable<Type> Inheritance(this Type input)
        {
            Type result = input;
            while (!result.Equals(typeof(object)))
            {
                result = result.BaseType;
                yield return result;
            }
        }

        public static bool IsGeneric<T>(this Type input)
        {
            foreach (var i in input.GenericTypeArguments)
            {
                if (i == typeof(T))
                    return true;
            }
            return false;
        }

        public static bool IsGenericOf<T>(this Type input)
        {
            foreach (var i in input.GenericTypeArguments)
            {
                if (i.IsSubclassOf(typeof(T)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether or not the type is <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type input)
        {
            if (!input.GetTypeInfo().IsGenericType)
                return false;

            return input.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}