using System;
using TaleWorlds.Library;

namespace WarCouncilModern
{
    public static class ReflectionHelpers
    {
        public static bool HasMethod(Type type, string name, Type[] paramTypes = null)
        {
            try
            {
                var m = paramTypes == null ? type.GetMethod(name) : type.GetMethod(name, paramTypes);
                ModLogger.Info($"Reflection check {type.FullName}.{name} => {(m != null ? "FOUND" : "MISSING")}");
                return m != null;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"Reflection error checking {type.FullName}.{name}", ex);
                return false;
            }
        }

        public static bool HasProperty(Type type, string name)
        {
            try
            {
                var p = type.GetProperty(name);
                ModLogger.Info($"Reflection check {type.FullName}.{name} property => {(p != null ? "FOUND" : "MISSING")}");
                return p != null;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"Reflection error checking property {type.FullName}.{name}", ex);
                return false;
            }
        }
    }
}