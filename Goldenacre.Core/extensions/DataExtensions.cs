using System.Collections.Generic;
using System.Data;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class DataExtensions
    {
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader @this)
        {
            while (@this.Read())
            {
                yield return @this;
            }
        }
    }
}