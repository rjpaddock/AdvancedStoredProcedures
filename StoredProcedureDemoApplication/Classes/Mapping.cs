using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Reflection;

namespace StoredProcedureDemoApplication.Classes
{
    public class Mapping
    {
        public static ObservableCollection<T> StoredProcedureToObservableCollection<T>(SqlCommand cmd)
        {
            var retval = new ObservableCollection<T>();


            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    retval.Add(DataReaderToObject<T>(reader));
                }
            }


            return retval;
        }

        public static T DataReaderToObject<T>(SqlDataReader reader)
        {
            var objectToMap = (T) Activator.CreateInstance(typeof(T));
            var type = objectToMap.GetType();

            for (var i = 0; i < reader.FieldCount; ++i)
            {
                //var prop = type.GetProperty(reader.GetName(i));
                //add ignore case property RJP 04.11.2018
                var prop = type.GetProperty(reader.GetName(i),
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    if (reader.IsDBNull(i))
                    {
                        prop.SetValue(objectToMap, null);
                    }
                    else
                    {
                        var value = reader.GetValue(i);
                        if (value is string)
                        {
                            value = ((string) value).Trim();
                        }

                        if (prop.PropertyType.Name == "Int32")
                        {
                            prop.SetValue(objectToMap, Convert.ToInt32(value));
                        }
                        else if (prop.PropertyType.Name == "Decimal")
                        {
                            prop.SetValue(objectToMap, Convert.ToDecimal(value));
                        }
                        else if (prop.PropertyType.Name == "Boolean")
                        {
                            prop.SetValue(objectToMap, Convert.ToBoolean(value));
                        }
                        else
                        {
                            prop.SetValue(objectToMap, value);
                        }
                    }
                }
            }

            return objectToMap;
        }
    }
}