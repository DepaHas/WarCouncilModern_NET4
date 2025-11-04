#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WarCouncilModern.Utilities;

namespace WarCouncilModern.Utilities.Reflection
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Attempts to get a private or public field value from an instance. Returns default(T) if not found.
        /// </summary>
        public static T? GetFieldValue<T>(object instance, string fieldName)
        {
            if (instance == null) return default;
            try
            {
                var t = instance.GetType();
                var field = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null) return default;
                var value = field.GetValue(instance);
                return value is T cast ? cast : default;
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.GetFieldValue failed for {fieldName} on {instance.GetType().FullName}", ex);
                return default;
            }
        }

        /// <summary>
        /// Attempts to set a private or public field value on an instance. Returns true on success.
        /// </summary>
        public static bool SetFieldValue(object instance, string fieldName, object? value)
        {
            if (instance == null) return false;
            try
            {
                var t = instance.GetType();
                var field = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null) return false;
                field.SetValue(instance, value);
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.SetFieldValue failed for {fieldName} on {instance.GetType().FullName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Attempts to get a property value (public or non-public). Returns default(T) if not found.
        /// </summary>
        public static T? GetPropertyValue<T>(object instance, string propertyName)
        {
            if (instance == null) return default;
            try
            {
                var t = instance.GetType();
                var prop = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop == null) return default;
                var value = prop.GetValue(instance);
                return value is T cast ? cast : default;
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.GetPropertyValue failed for {propertyName} on {instance.GetType().FullName}", ex);
                return default;
            }
        }

        /// <summary>
        /// Attempts to set a property value (public or non-public). Returns true on success.
        /// </summary>
        public static bool SetPropertyValue(object instance, string propertyName, object? value)
        {
            if (instance == null) return false;
            try
            {
                var t = instance.GetType();
                var prop = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop == null || !prop.CanWrite) return false;
                prop.SetValue(instance, value);
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.SetPropertyValue failed for {propertyName} on {instance.GetType().FullName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Invokes an instance method (public or non-public) with the given arguments. Returns result or default if failed.
        /// </summary>
        public static object? InvokeMethod(object instance, string methodName, params object?[] args)
        {
            if (instance == null) return null;
            try
            {
                var t = instance.GetType();
                var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                               .Where(m => m.Name == methodName)
                               .ToArray();
                if (methods.Length == 0) return null;

                // choose overload by parameter count and compatibility
                MethodInfo? target = null;
                foreach (var m in methods)
                {
                    var parms = m.GetParameters();
                    if (parms.Length != args.Length) continue;
                    bool ok = true;
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var pType = parms[i].ParameterType;
                        var a = args[i];
                        if (a == null) continue;
                        if (!pType.IsAssignableFrom(a.GetType())) { ok = false; break; }
                    }
                    if (ok) { target = m; break; }
                }

                target ??= methods.FirstOrDefault();
                if (target == null) return null;

                return target.Invoke(instance, args);
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.InvokeMethod failed for {methodName} on {instance.GetType().FullName}", ex);
                return null;
            }
        }

        /// <summary>
        /// Safely gets a static property value from a type full name. Returns default(T) if not found.
        /// </summary>
        public static T? GetStaticPropertyValue<T>(string typeFullName, string propertyName)
        {
            try
            {
                var type = Type.GetType(typeFullName);
                if (type == null) return default;
                var prop = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop == null) return default;
                var value = prop.GetValue(null);
                return value is T cast ? cast : default;
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.GetStaticPropertyValue failed for {typeFullName}.{propertyName}", ex);
                return default;
            }
        }

        /// <summary>
        /// Tries to call a static method and return its result, null on failure.
        /// </summary>
        public static object? InvokeStaticMethod(string typeFullName, string methodName, params object?[] args)
        {
            try
            {
                var type = Type.GetType(typeFullName);
                if (type == null) return null;
                var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (method == null) return null;
                return method.Invoke(null, args);
            }
            catch (Exception ex)
            {
                GlobalLog.Instance.Error($"ReflectionHelpers.InvokeStaticMethod failed for {typeFullName}.{methodName}", ex);
                return null;
            }
        }

        /// <summary>
        /// Helper to safely enumerate an IEnumerable<object?> into a typed list without throwing on null input.
        /// </summary>
        public static List<T> SafeToList<T>(IEnumerable<T>? source) =>
            source != null ? source.ToList() : new List<T>();
    }
}