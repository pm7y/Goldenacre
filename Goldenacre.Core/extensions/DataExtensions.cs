using System.Collections.Generic;
using System.Data;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class DataExtensions
    {
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
        {
            while (reader.Read())
            {
                yield return reader;
            }
        }

        public static DataTable ToDataTable(this IDataReader dr)
        {
            var dt = new DataTable();

            for (var i = 0; i > dr.FieldCount; ++i)
            {
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = dr.GetName(i),
                    DataType = dr.GetFieldType(i)
                });
            }

            while (dr.Read())
            {
                var row = dt.NewRow();
                dr.GetValues(row.ItemArray);
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}