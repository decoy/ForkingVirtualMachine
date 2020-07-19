namespace ForkingVirtualMachine.Store.Database.Migrations
{
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Utility;

    /// <summary>
    /// handles the fun case of first startup.
    /// </summary>
    public static class Initialization
    {
        public static async Task CreateDatabase(DbConnection db, string dbName)
        {
            if (!await DatabaseExists(db, dbName))
            {
                await CreateDb(db, dbName);
            }
        }

        public static async Task DropDatabase(DbConnection db, string dbName)
        {
            if (await DatabaseExists(db, dbName))
            {
                await DropDb(db, dbName);
            }
        }

        public static async Task CreateMigrationsTable(DbConnection db)
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS migrations (
                    id     INTEGER  PRIMARY KEY AUTOINCREMENT
                                    UNIQUE
                                    NOT NULL,
                    name   TEXT     UNIQUE
                                    NOT NULL,
                    ran_on DATETIME NOT NULL
                );";

            using (var trans = db.BeginTransaction())
            {
                await db.Execute(sql);
                trans.Commit();
            }
        }

        private static async Task<bool> DatabaseExists(DbConnection db, string dbName)
        {
            const string sql = @"SELECT 1 
                FROM pg_database 
                WHERE datname = @dbName;";

            var x = await db.Query(sql, new { dbName }, (r) => (int)r[0]);

            return x.Any();
        }

        private static async Task CreateDb(DbConnection db, string dbName)
        {
            var sql = $@"
                CREATE DATABASE {dbName}
                WITH OWNER = postgres
                ENCODING = 'UTF8'
                CONNECTION LIMIT = -1;";

            await db.Execute(sql);
        }

        private static async Task DropDb(DbConnection db, string dbName)
        {
            var sql = $@"DROP DATABASE {dbName};";

            await db.Execute(sql);
        }
    }
}
