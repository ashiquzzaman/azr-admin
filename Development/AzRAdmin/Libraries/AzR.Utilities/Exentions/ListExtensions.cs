using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace AzR.Utilities.Exentions
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            var props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                                 prop.PropertyType);
            }


            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DataTable CreateDataTable<T>(this IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();


            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }


            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }


                dataTable.Rows.Add(values);
            }


            return dataTable;
        }
    }
}
