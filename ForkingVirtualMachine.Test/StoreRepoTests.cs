using ForkingVirtualMachine.Machines;
using ForkingVirtualMachine.Store.Database;
using ForkingVirtualMachine.Store.Database.Migrations;
using ForkingVirtualMachine.Store.Models;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class StoreRepoTests
    {
        public static async Task<DbConnection> OpenDb()
        {
            var db = new SqliteConnection("Data Source=:memory:");
            await db.OpenAsync();
            await Migrator.Run(db);
            return db;
        }

        [TestMethod]
        public async Task Migrates()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);
                var ran = (await repo.GetMigrations()).ToList();

                Assert.AreEqual(1, ran.Count);
                Assert.AreEqual(typeof(M20200620_Create).Name, ran[0].name);
            }
        }

        [TestMethod]
        public async Task Nodes()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);

                var n1 = new Node()
                {

                };

                await repo.InsertNode(n1);
            }
        }
    }
}
