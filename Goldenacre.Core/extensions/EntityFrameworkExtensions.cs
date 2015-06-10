using System;
using System.Linq;
using System.Reflection;

public static class EntityFrameworkExtensions
{
    public static void ConvertDateTimePropertiesToUtc<T>(this object o) where T : class
    {
        if (o != null)
        {
            var properties =
                o.GetType()
                    .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                    .Where(
                        x =>
                            x.Name.ToLowerInvariant().Contains("utc") &&
                            (x.PropertyType == typeof (DateTime) || x.PropertyType == typeof (DateTime?)));

            foreach (var property in properties)
            {
                var dt = property.PropertyType == typeof (DateTime?)
                    ? (DateTime?) property.GetValue(o, null)
                    : (DateTime) property.GetValue(o, null);

                if (dt != null)
                {
                    var v = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
                    property.SetValue(o, v, null);
                }
            }
        }
    }
}