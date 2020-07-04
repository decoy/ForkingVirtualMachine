//namespace ForkingVirtualMachine.Store.Database
//{
//    using Config;
//    using Microsoft.Extensions.Configuration;
//    using Migrations;
//    using Npgsql;
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using Utility;

//    public class Migrator
//    {
//        private readonly IConfiguration config;

//        public Migrator(IConfiguration config)
//        {
//            this.config = config;
//        }

//        public async Task Run()
//        {
//            var dbConnection = config.GetDatabaseConnection();
//            var dbName = config.GetDatabaseName();
//            var connectionString = config.GetConnectionString();

//            using (var db = new NpgsqlConnection(dbConnection))
//            {
//                await db.OpenAsync();
//                await Initialization.CreateDatabase(db, dbName);
//            }

//            using (var db = new NpgsqlConnection(connectionString))
//            {
//                await db.OpenAsync();

//                await Initialization.CreateMigrationsTable(db);

//                var repo = new Repository(db);

//                var ran = await repo.GetMigrations();

//                var migrations = GetMigrators()
//                    .Where(m => !ran.Any(r => r.name == m.GetType().Name))
//                    .ToList();

//                foreach (var m in migrations)
//                {
//                    await m.Up(db);
//                    await repo.InsertMigration(m.GetType().Name, DateTime.UtcNow);
//                }
//            }
//        }

//        public async Task Drop()
//        {
//            var dbConnection = config.GetDatabaseConnection();
//            var dbName = config.GetDatabaseName();

//            using (var db = new NpgsqlConnection(dbConnection))
//            {
//                await db.OpenAsync();
//                await Initialization.DropDatabase(db, dbName);
//            }
//        }

//        private static IEnumerable<IMigrate> GetMigrators()
//        {
//            return typeof(IMigrate)
//                .Assembly
//                .GetExportedTypes()
//                .Where(t => t.Implements<IMigrate>())
//                .Where(t => t.IsImplementedType())
//                .OrderBy(t => t.Name)
//                .Select(CreateMigrator);
//        }

//        private static IMigrate CreateMigrator(Type type)
//        {
//            return (IMigrate)Activator.CreateInstance(type);
//        }
//    }
//}
