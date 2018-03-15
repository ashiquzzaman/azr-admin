using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using AzR.Utilities.Attributes;
using AzR.Utilities.Models;

namespace AzR.Utilities.Exentions
{
    public static class ObjectExtentions
    {
        public static IEnumerable<ObjectChangeLog> ToChangeLog(this object newModel)
        {
            var properties = newModel.GetType().GetProperties();
            return (from property in properties
                    let newValue = property != null
                                   && !Attribute.IsDefined(property, typeof(IgnoreLogAttribute))
                                   && property.GetValue(newModel) != null
                        ? property.GetValue(newModel).ToString()
                        : null
                    select new ObjectChangeLog
                    {
                        FieldName = property.Name,
                        ValueBefore = null,
                        ValueAfter = newValue
                    }).ToList();
        }

        public static IEnumerable<ObjectChangeLog> Compare(this object newModel, object oldModel)
        {
            var properties = oldModel.GetType().GetProperties();
            return (from property in properties
                    let oldValue = property != null
                                   && !Attribute.IsDefined(property, typeof(IgnoreLogAttribute))
                                   && property.GetValue(oldModel) != null
                        ? property.GetValue(oldModel).ToString()
                        : null
                    let newValue = property != null
                                   && !Attribute.IsDefined(property, typeof(IgnoreLogAttribute))
                                   && property.GetValue(newModel) != null
                        ? property.GetValue(newModel).ToString()
                        : null
                    where oldValue != newValue
                    select new ObjectChangeLog
                    {
                        FieldName = property.Name,
                        ValueBefore = oldValue,
                        ValueAfter = newValue
                    }).ToList();
        }

        public static Dictionary<string, object> RemoveDefault<T>(this T obj)
        {
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in obj.GetType().GetProperties())
            {
                var val = pr.GetValue(obj);
                if (val == null)
                    continue;
                if (val is string && (string.IsNullOrWhiteSpace(val.ToString()) || string.IsNullOrEmpty(val.ToString())))
                    continue;
                if (val is int && (int)val == default(int))
                    continue;
                if (val is long && (long)val == default(long))
                    continue;
                if (val is double && Math.Abs((double)val - default(double)) <= 0.00)
                    continue;
                if (val is decimal && (decimal)val == default(decimal))
                    continue;
                if (val is DateTime && (DateTime)val == default(DateTime))
                    continue;
                returnClass.Add(pr.Name, val);
            }
            return new Dictionary<string, object>(returnClass);
        }

        public static Dictionary<string, object> RemoveUnused<T>(this T obj)
        {
            var t = obj.GetType();
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in t.GetProperties())
            {
                var val = pr.GetValue(obj);
                if (val == null)
                    continue;
                if (val is string && string.IsNullOrWhiteSpace(val.ToString()))
                    continue;
                if (val is int && Convert.ToInt32(val) == 0)
                    continue;
                if (val is long && Convert.ToInt64(val) == 0)
                    continue;
                if (val is double && Math.Abs(Convert.ToDouble(val)) <= 0.0)
                    continue;
                if (val is decimal && Convert.ToDecimal(val) == 0)
                    continue;
                if (val is DateTime && (DateTime)val == default(DateTime))
                    continue;
                returnClass.Add(pr.Name, val);
            }
            return new Dictionary<string, object>(returnClass);
        }

        public static Dictionary<string, object> RemoveProperty<T>(this T obj, params string[] properties)
        {
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in obj.GetType().GetProperties())
            {
                foreach (var property in properties)
                {
                    if (pr.Name == property) continue;
                    var val = pr.GetValue(obj);
                    returnClass.Add(pr.Name, val);
                }
            }
            return new Dictionary<string, object>(returnClass);
        }

        public static Dictionary<string, object> RemoveExcept<T>(this T obj, params string[] properties)
        {
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in obj.GetType().GetProperties())
            {
                foreach (var property in properties)
                {
                    if (pr.Name != property) continue;
                    var val = pr.GetValue(obj);
                    returnClass.Add(pr.Name, val);
                }
            }
            return new Dictionary<string, object>(returnClass);
        }

        public static bool InstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = typeof(T);
                var ignoreList = new List<string>(ignore);
                var unequalProperties =
                    from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignoreList.Contains(pi.Name)
                    let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
                    let toValue = type.GetProperty(pi.Name).GetValue(to, null)
                    where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                    select selfValue;
                return !unequalProperties.Any();
            }
            return self == to;
        }

        public static bool PropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return self == to;
        }

        public static string ObjectToStringWithType<T>(this T obj)
        {
            var strNew = string.Empty;

            if ((obj == null)) return strNew;
            var aPropertyInfo = obj.GetType().GetProperties();

            return aPropertyInfo.Aggregate(strNew,
                (current, aProperty) =>
                    string.IsNullOrEmpty(current)
                        ? aProperty.Name + " (" + aProperty.PropertyType + ") " + ": " + aProperty.GetValue(obj, null)
                        : current + "; " + aProperty.Name + " (" + aProperty.PropertyType + ") " + ": " +
                          aProperty.GetValue(obj, null));
        }

        public static string ObjectToString<T>(this T obj)
        {
            var strNew = string.Empty;

            if ((obj == null)) return strNew;
            var aPropertyInfo = obj.GetType().GetProperties();

            return aPropertyInfo.Aggregate(strNew,
                (current, aProperty) =>
                    string.IsNullOrEmpty(current)
                        ? aProperty.Name + ": " + aProperty.GetValue(obj, null)
                        : current + "; " + aProperty.Name + ": " + aProperty.GetValue(obj, null));
        }

        public static string ValuesToString<T>(this T obj)
        {
            var strNew = string.Empty;

            if ((obj == null)) return strNew;
            var aPropertyInfo = obj.GetType().GetProperties();

            return aPropertyInfo.Aggregate(strNew,
                (current, aProperty) =>
                    string.IsNullOrEmpty(current)
                        ? aProperty.GetValue(obj, null).ToString()
                        : current + "; " + aProperty.GetValue(obj, null));
        }

        public static string PropertiesToString<T>(this T obj)
        {
            var strNew = string.Empty;

            if ((obj == null)) return strNew;
            var aPropertyInfo = obj.GetType().GetProperties();

            return aPropertyInfo.Aggregate(strNew,
                (current, aProperty) => string.IsNullOrEmpty(current) ? aProperty.Name : current + "; " + aProperty.Name);
        }

        public static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(source, null));
        }

        public static bool IsOfType<T>(this object value)
        {
            return value is T;
        }
        public static bool IsNullOrDefault<T>(this T argument)
        {

            if (argument == null) return true;
            if (Equals(argument, default(T))) return true;


            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;


            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }

        public static T SetDefaultValue<T>(this T objectValue)
        {

            foreach (var prop in objectValue.GetType().GetProperties())
            {
                var propertyType = prop.PropertyType.FullName;
                var propertyValue = prop.GetValue(objectValue, null);
                if (propertyValue != null) continue;
                if (propertyType.Contains("System.String"))
                    prop.SetValue(objectValue, "");
                else if (propertyType.Contains("System.Boolean"))
                    prop.SetValue(objectValue, false);
                else if (propertyType.Contains("System.Int"))
                    prop.SetValue(objectValue, 0);
                else if (propertyType.Contains("System.DateTime"))
                    prop.SetValue(objectValue, DateTime.UtcNow.Date);
                else if (propertyType.Contains("System.Double"))
                    prop.SetValue(objectValue, 0.0d);
                else if (propertyType.Contains("System.Decimal"))
                    prop.SetValue(objectValue, 0.0M);
                else
                    prop.SetValue(objectValue, 0.0);
            }
            return objectValue;
        }

        public static DataTable ToDataTable<TSource>(this object data)
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            var props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                                 prop.PropertyType);
            }

            var values = new object[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(data, null);
            }
            dataTable.Rows.Add(values);

            return dataTable;
        }
    }
}
