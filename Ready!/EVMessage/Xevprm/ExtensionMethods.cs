using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal static class ExtensionMethods
{

    internal static bool In<T>(this T entity, params T[] entities)
    {
        if (entities == null || entities.Count() == 0) return false;

        if (entities.Contains(entity))
            return true;
        return false;
    }

    internal static bool NotIn<T>(this T entity, params T[] entities)
    {
        if (entities == null || entities.Count() == 0) return true;

        if (entities.Contains(entity))
            return false;
        return true;
    }

    internal static string TrimIfNotNull(this string value)
    {
        return value != null ? value.Trim() : null;
    }
}
