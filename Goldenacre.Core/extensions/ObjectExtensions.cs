using System;
using System.Globalization;
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
        public static string ToUpperInvariant<T>(this T @this) where T : class
        {
            return @this.ToString().ToUpperInvariant();
        }

        public static string ToLowerInvariant<T>(this T @this) where T : class
        {
            return @this.ToString().ToLowerInvariant();
        }

        public static T EnsureBetween<T>(this T @this, T min, T max) where T : IComparable<T>
        {
            if (@this.CompareTo(min) < 0)
            {
                return min;
            }

            if (@this.CompareTo(max) > 0)
            {
                return max;
            }

            return @this;
        }

        public static bool IsTruthy<T>(this T @this)
        {
            if (@this == null)
            {
                return false;
            }

            if (typeof (T) == typeof (bool))
            {
                return Convert.ToBoolean(Convert.ChangeType(@this, typeof (bool)));
            }

            var s = Convert.ToString(@this);

            decimal num;
            if (s.IsNumeric() && decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out num))
            {
                return num > 0;
            }

            if (!string.IsNullOrWhiteSpace(s))
            {
                return s.EqualsAnyCI("true", "1", "y", "yes", "ok", "+");
            }

            return false;
        }

        public static bool IsSet(this Enum @this, Enum matchTo)
        {
            return (Convert.ToUInt32(@this) & Convert.ToUInt32(matchTo)) != 0;
        }

        public static T DeepClone<T>(this T @this) where T : ISerializable
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, @this);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }

        public static bool IsBetween<T>(this T @this, T from, T to) where T : IComparable<T>
        {
            return @this.CompareTo(from) >= 0 && @this.CompareTo(to) <= 0;
        }

        public static bool IsNullable<T>(this T @this)
        {
            if (@this == null) return true; // obvious
            var type = typeof (T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        /// <summary>
        ///     Calls DateTime.SpecifyKind on all DateTime or DateTime? properties and changed to UTC.
        ///     This is useful after an object has been materialozed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="criteria"></param>
        public static void ConvertDateTimePropertiesToUtc<T>(
            this T @this,
            Expression<Func<PropertyInfo, bool>> criteria = null
            ) where T : class
        {
            if (@this != null)
            {
                var properties =
                    @this.GetType()
                        .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                        .Where(
                            x =>
                                x.PropertyType == typeof (DateTime) || x.PropertyType == typeof (DateTime?));

                if (criteria != null)
                {
                    properties = properties.Where(criteria.Compile());
                }

                foreach (var property in properties)
                {
                    var dt = property.PropertyType == typeof (DateTime?)
                        ? (DateTime?) property.GetValue(@this, null)
                        : (DateTime) property.GetValue(@this, null);

                    if (dt != null && dt.Value.Kind == DateTimeKind.Unspecified)
                    {
                        var v = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
                        property.SetValue(@this, v, null);
                    }
                    else if (dt != null && dt.Value.Kind == DateTimeKind.Local)
                    {
                        var v = dt.Value.ToUniversalTime();
                        property.SetValue(@this, v, null);
                    }
                }
            }
        }
    }
}