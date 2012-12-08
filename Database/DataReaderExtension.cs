using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Database
{
    public static class DataReaderExtension
    {
        public static bool HasColumn(this IDataRecord record, string column)
        {
            if (record == null) throw new ArgumentNullException("record");

            for (int i = 0; i < record.FieldCount; ++i)
            {
                if (record.GetName(i).Equals(column))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get type safe value from specified column.
        /// Returns empty string for non-existent (null) string values.
        /// </summary>
        public static T Get<T>(this IDataRecord record, string column)
        {
            if (record == null) throw new ArgumentNullException("record");

            if (!record.HasColumn(column) || record[column].GetType() == typeof(DBNull))
            {
                if (typeof(T).Equals(typeof(string)))
                    return (T)(object)"";
                else
                    return default(T);
            }

            if (record[column] != DBNull.Value &&
                record[column].GetType() != typeof(T))
            {
                throw new InvalidCastException("Cannot cast " + column + " from " + record[column].GetType().Name + " to " + typeof(T).Name);
            }

            if (record[column] == DBNull.Value && typeof(T) == typeof(string))
                return (T)(object)"";

            return (T)record[column];
        }
    }
}
