using System;

namespace Taga.DynamicServices
{
    public static class ReflectionExtensions
    {
        public static object CreateInstance(this Type type)
        {
            return type.HasDefaultConstrutor()
                       ? type.CreateInstance(new object[0])
                       : type.GetDefaultValue();
        }

        public static object CreateInstance(this Type type, object[] ctorArgs)
        {
            return type.IsArray
                       ? Array.CreateInstance(type.GetElementType(), 0)
                       : Activator.CreateInstance(type, ctorArgs);
        }

        public static Type GetPropertyType(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName).PropertyType;
        }

        public static object GetPropertyValue(this Type type, string propertyName, object instance)
        {
            return type.GetProperty(propertyName).GetValue(instance);
        }

        public static void SetPropertyValue(this Type type, string propertyName, object instance, object value)
        {
            type.GetProperty(propertyName).SetValue(instance, value);
        }

        public static object InvokeMethod(this Type type, string method, object instance)
        {
            return type.InvokeMethod(method, instance, null);
        }

        public static object InvokeMethod(this Type type, string method, object instance, object[] args)
        {
            return type.GetMethod(method).Invoke(instance, args);
        }

        public static bool HasDefaultConstrutor(this Type type)
        {
            return null != type.GetConstructor(Type.EmptyTypes);
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type == typeof(void))
                return null;

            return typeof(ReflectionExtensions).GetMethod("Default").MakeGenericMethod(type).Invoke(null, null);
        }

        public static T Default<T>()
        {
            return default(T);
        }
    }
}
