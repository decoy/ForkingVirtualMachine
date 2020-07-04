namespace ForkingVirtualMachine.Store.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;

    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds parameters using the names and values of the properties of the passed in object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbCommand AddParameters<T>(this DbCommand command, T parameters)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                command.AddParameterWithValue(prop.Name, prop.GetValue(parameters));
            }
            return command;
        }

        /// <summary>
        /// Add with value equivalent on the base <see cref="DbCommand"/> type
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public static DbCommand AddParameterWithValue(this DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue == null ? DBNull.Value : parameterValue;
            command.Parameters.Add(parameter);
            return command;
        }

        public static Task<int> Execute(this DbCommand cmd, string sql)
        {
            cmd.CommandText = sql;
            return cmd.ExecuteNonQueryAsync();
        }

        public static Task<int> Execute<TParameters>(this DbCommand cmd, string sql, TParameters parameters)
        {
            cmd.CommandText = sql;
            cmd.AddParameters(parameters);
            return cmd.ExecuteNonQueryAsync();
        }

        public static async Task<TResult> QuerySingle<TResult>(this DbCommand cmd, string sql)
        {
            cmd.CommandText = sql;

            var result = await cmd.ExecuteScalarAsync();
            return (TResult)result;
        }

        public static async Task<TResult> QuerySingle<TParameters, TResult>(this DbCommand cmd, string sql, TParameters parameters)
        {
            cmd.CommandText = sql;
            cmd.AddParameters(parameters);

            var result = await cmd.ExecuteScalarAsync();
            return (TResult)result;
        }

        public static async Task<TResult> QuerySingle<TResult>(this DbCommand cmd, string sql, Func<DbDataReader, TResult> mapper)
        {
            cmd.CommandText = sql;

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.ReadSingle(mapper);
            }
        }

        public static async Task<TResult> QuerySingle<TParameters, TResult>(this DbCommand cmd, string sql, TParameters parameters, Func<DbDataReader, TResult> mapper)
        {
            cmd.CommandText = sql;
            cmd.AddParameters(parameters);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.ReadSingle(mapper);
            }
        }

        public static async Task<IEnumerable<TResult>> Query<TResult>(this DbCommand cmd, string sql, Func<DbDataReader, TResult> mapper)
        {
            cmd.CommandText = sql;

            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.ReadMany(mapper);
            }
        }

        public static async Task<IEnumerable<TResult>> Query<TParameters, TResult>(this DbCommand cmd, string sql, TParameters parameters, Func<DbDataReader, TResult> mapper)
        {
            cmd.CommandText = sql;
            cmd.AddParameters(parameters);

            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.ReadMany(mapper);
            }
        }
    }
}
