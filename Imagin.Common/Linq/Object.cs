using System;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Returns object as specified type.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type As<Type>(this object source) => source is Type ? (Type)source : default;

        /// ---------------------------------------------------------------------------------------------

        public static void If<Type>(this object source, Action<Type> action)
        {
            if (source is Type)
                action((Type)source);
        }

        /// ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Checks if given object's type implements interface (T).
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Implements<Type>(this object source) where Type : class => (bool)source.GetType()?.GetTypeInfo()?.Implements<Type>();

        /// ---------------------------------------------------------------------------------------------

        public static decimal Decimal(this object i) => Convert.ToDecimal(i);

        public static double Double(this object i) => Convert.ToDouble(i);

        public static short Int16(this object i) => Convert.ToInt16(i);

        public static int Int32(this object i) => Convert.ToInt32(i);

        public static long Int64(this object i) => Convert.ToInt64(i);

        public static float Single(this object i) => Convert.ToSingle(i);

        /// ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is<T>(this object input) => input is T;

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool IsAny(this object input, params Type[] types)
        {
            var t = input.GetType();
            foreach (var i in types)
            {
                if (t == i || t.GetTypeInfo().IsSubclassOf(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if specified object is NOT of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Not<T>(this object input) => !input.Is<T>();

        /// ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Checks if specified object is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Null(this object value) => value == null;

        /// <summary>
        /// Gets whether or not the <see cref="object"/> is <see cref="System.Nullable"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Nullable(this object value) => value.Nullable<object>();

        /// ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Get the value of the given field.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue(this object input, string fieldName) => input.GetType().GetField(fieldName).GetValue(input);

        /// <summary>
        /// Get the value of the given property.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object input, string propertyName) => input.GetType().GetProperty(propertyName).GetValue(input, null);

        /// ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Set value of given field.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue<Type>(this object input, string fieldName, Type value)
        {
            var field = input.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            field.SetValue(input, value);
        }

        /// <summary>
        /// Set value of given property.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="input"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue<Type>(this object input, string propertyName, Type value)
        {
            var property = input.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property?.CanWrite == true)
                property.SetValue(input, value, null);
        }

        /// ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Cast object to given type.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Type To<Type>(this object value) => (Type)value;
    }
}
