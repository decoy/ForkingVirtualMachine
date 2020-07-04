namespace ForkingVirtualMachine.Store.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Some helper extensions for the System.Data.Common abstractions
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Gets the reader's current field values and maps them to matching properties on the target.
        /// Does not do any type checking, so will throw if you do something silly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T MapTo<T>(this IDataReader reader, T target)
        {
            var values = reader.GetFieldValues();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (values.ContainsKey(prop.Name))
                {
                    prop.SetValue(target, values[prop.Name]);
                }
            }
            return target;
        }

        /// <summary>
        /// Gets the reader's current row values in dictionary (name/value pairs) format.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetFieldValues(this IDataReader reader)
        {
            var d = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                d.Add(reader.GetName(i), reader.GetValue(i));
            }
            return d;
        }

        /// <summary>
        /// Reads async then maps the results. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<TResult> ReadSingle<TResult>(this DbDataReader reader, Func<DbDataReader, TResult> mapper)
        {
            while (await reader.ReadAsync())
            {
                return mapper(reader);
            }
            return default;
        }

        /// <summary>
        /// Reads async and yield returns the mapped results.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<TResult> Read<TResult>(this DbDataReader reader, Func<DbDataReader, TResult> mapper)
        {
            while (await reader.ReadAsync())
            {
                yield return mapper(reader);
            }
        }

        /// <summary>
        /// Maps all the rows to a list.
        /// Note: Because this is async, it's not using yield and will have a slight cost (creating a list) compared to mapping a single item.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResult>> ReadMany<TResult>(this DbDataReader reader, Func<DbDataReader, TResult> mapper)
        {
            if (!reader.HasRows) { return Enumerable.Empty<TResult>(); }

            var list = new List<TResult>();

            while (await reader.ReadAsync())
            {
                list.Add(mapper(reader));
            }

            return list;
        }

        //public static TValue? GetNullableValue<TValue>(this IDataReader record, string name) where TValue : struct
        //{
        //    return record.GetValue<TValue, TValue?>(name);
        //}

        public static TResult GetValue<TResult>(this IDataReader reader, string name)
        {
            var result = reader[name];

            return !result.Equals(DBNull.Value)
                ? (TResult)result
                : default;
        }
    }
}
