namespace ForkingVirtualMachine.Store.Database
{
    using Microsoft.Data.Sqlite;
    using Migrations;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Utility;

    public class Migrator
    {
        private readonly string connectionString;

        public Migrator(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task Run()
        {
            using (var db = new SqliteConnection(connectionString))
            {
                await db.OpenAsync();
                await Run(db);
            }
        }

        public static async Task Run(DbConnection db)
        {
            await Initialization.CreateMigrationsTable(db);

            var repo = new Repository(db);

            var ran = await repo.GetMigrations();

            var migrations = GetMigrators()
                .Where(m => !ran.Any(r => r.Name == m.GetType().Name))
                .ToList();

            foreach (var m in migrations)
            {
                await m.Up(db);
                await repo.InsertMigration(m.GetType().Name, DateTime.UtcNow);
            }
        }

        private static IEnumerable<IMigrate> GetMigrators()
        {
            return typeof(IMigrate)
                .Assembly
                .GetExportedTypes()
                .Where(t => t.Implements<IMigrate>())
                .Where(t => t.IsImplementedType())
                .OrderBy(t => t.Name)
                .Select(CreateMigrator);
        }

        private static IMigrate CreateMigrator(Type type)
        {
            return (IMigrate)Activator.CreateInstance(type);
        }
    }
}
