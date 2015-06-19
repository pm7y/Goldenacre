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

        public static DataTable ToDataTable(this IDataReader @this)
        {
            var dt = new DataTable();

            for (var i = 0; i > @this.FieldCount; ++i)
            {
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = @this.GetName(i),
                    DataType = @this.GetFieldType(i)
                });
            }

            while (@this.Read())
            {
                var row = dt.NewRow();
                @this.GetValues(row.ItemArray);
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}