namespace ForkingVirtualMachine.Store.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper for working with the connection object
    /// Mostly just creates/disposes of commands and passes on the actions
    /// </summary>
    public static class DbConnectionExtensions
    {
        public static async Task<TResult> DoCommand<TResult>(this DbConnection connection, Func<DbCommand, Task<TResult>> action)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await action(cmd);
            }
        }

        public static async Task<int> Execute(this DbConnection connection, string sql)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.Execute(sql);
            }
        }

        public static async Task<int> Execute<TParameters>(this DbConnection connection, string sql, TParameters parameters)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.Execute(sql, parameters);
            }
        }

        public static async Task<TResult> QuerySingle<TResult>(this DbConnection connection, string sql)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.QuerySingle<TResult>(sql);
            }
        }

        public static async Task<TResult> QuerySingle<TParameters, TResult>(this DbConnection connection, string sql, TParameters parameters)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.QuerySingle<TParameters, TResult>(sql, parameters);
            }
        }

        public static async Task<TResult> QuerySingle<TResult>(this DbConnection connection, string sql, Func<DbDataReader, TResult> mapper)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.QuerySingle(sql, mapper);
            }
        }

        public static async Task<TResult> QuerySingle<TParameters, TResult>(this DbConnection connection, string sql, TParameters parameters, Func<DbDataReader, TResult> mapper)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.QuerySingle(sql, parameters, mapper);
            }
        }

        public static async Task<IEnumerable<TResult>> Query<TResult>(this DbConnection connection, string sql, Func<DbDataReader, TResult> mapper)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.Query(sql, mapper);
            }
        }

        public static async Task<IEnumerable<TResult>> Query<TParameters, TResult>(this DbConnection connection, string sql, TParameters parameters, Func<DbDataReader, TResult> mapper)
        {
            using (var cmd = connection.CreateCommand())
            {
                return await cmd.Query(sql, parameters, mapper);
            }
        }
    }
}
