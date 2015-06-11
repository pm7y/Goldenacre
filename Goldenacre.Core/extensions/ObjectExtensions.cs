using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ObjectExtensions
    {
        public static T EnsureBetween<T>(this T o, T min, T max) where T : IComparable<T>, IComparable
        {
            if (o.CompareTo(min) < 0)
            {
                return min;
            }

            if (o.CompareTo(max) > 0)
            {
                return max;
            }

            return o;
        }

        public static bool Truthy<T>(this T input)
        {
            if (input == null)
            {
                return false;
            }

            if (typeof (T) == typeof (bool))
            {
                return Convert.ToBoolean(Convert.ChangeType(input, typeof (bool)));
            }

            var asString = Convert.ToString(input);

            if (!string.IsNullOrWhiteSpace(asString))
            {
                return asString.EqualsAny("true", "1", "y", "yes", "ok");
            }

            return false;
        }

        public static bool IsSet(this Enum input, Enum matchTo)
        {
            return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
        }

        public static T DeepClone<T>(this T input) where T : ISerializable
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }

        public static bool In<T>(this T value, params T[] list)
        {
            return list.Contains(value);
        }

        public static bool Between<T>(this T value, T from, T to) where T : IComparable<T>, IComparable
        {
            return value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
        }

        public static bool IsNullable<T>(this T obj)
        {
            if (obj == null) return true; // obvious
            var type = typeof (T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        public static bool IsType<T>(this object item) where T : class
        {
            return item is T;
        }

        public static bool IsNotType<T>(this object item) where T : class
        {
            return !(item.IsType<T>());
        }

        /// <summary>
        ///     Calls DateTime.SpecifyKind on all DateTime or DateTime? properties and changed to UTC.
        ///     This is useful after an object has been materialozed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="criteria"></param>
        public static void ConvertDateTimePropertiesToUtc<T>(this T o,
            Expression<Func<PropertyInfo, bool>> criteria = null) where T : class
        {
            if (o != null)
            {
                var properties =
                    o.GetType()
                        .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                        .Where(
                            x =>
                                (x.PropertyType == typeof (DateTime) || x.PropertyType == typeof (DateTime?)));

                if (criteria != null)
                {
                    properties = properties.Where(criteria.Compile());
                }

                foreach (var property in properties)
                {
                    var dt = property.PropertyType == typeof (DateTime?)
                        ? (DateTime?) property.GetValue(o, null)
                        : (DateTime) property.GetValue(o, null);

                    if (dt != null && dt.Value.Kind != DateTimeKind.Utc)
                    {
                        var v = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
                        property.SetValue(o, v, null);
                    }
                }
            }
        }
    }
}