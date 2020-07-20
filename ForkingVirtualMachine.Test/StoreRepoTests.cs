using ForkingVirtualMachine.Store.Database;
using ForkingVirtualMachine.Store.Database.Migrations;
using ForkingVirtualMachine.Store.Models;
using ForkingVirtualMachine.Utility;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class StoreRepoTests
    {
        static ArrayEqualityComparer<byte> comparer = new ArrayEqualityComparer<byte>();

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

                var d1 = new Content()
                {
                    Id = Guid.NewGuid().ToByteArray(),
                    Data = Guid.NewGuid().ToByteArray(),
                };

                Assert.AreEqual(1, await repo.InsertContent(d1));

                var ds1 = await repo.GetContent(d1.Id);

                Assert.IsTrue(comparer.Equals(d1.Id, ds1.Id));
                Assert.IsTrue(comparer.Equals(d1.Data, ds1.Data));

                var n1 = new Node()
                {
                    Id = Guid.NewGuid().ToByteArray(),
                    ParentId = null,
                    DataId = d1.Id,
                    Word = Guid.NewGuid().ToByteArray(),
                    Weight = 5,
                    ModifiedOn = DateTime.UtcNow,
                    Version = 0,
                };

                Assert.AreEqual(1, await repo.InsertNode(n1));

                var ns1 = await repo.GetNode(n1.Id);

                Assert.IsTrue(comparer.Equals(n1.Id, ns1.Id));
                Assert.IsTrue(comparer.Equals(n1.ParentId, ns1.ParentId));
                Assert.IsTrue(comparer.Equals(n1.DataId, ns1.DataId));
                Assert.IsTrue(comparer.Equals(n1.Word, ns1.Word));
                Assert.AreEqual(n1.Weight, ns1.Weight);
                Assert.AreEqual(n1.ModifiedOn, ns1.ModifiedOn);
                Assert.AreEqual(n1.Version, ns1.Version);

                var n2 = new Node()
                {
                    Id = Guid.NewGuid().ToByteArray(),
                    ParentId = n1.Id,
                    DataId = d1.Id,
                    Word = Guid.NewGuid().ToByteArray(),
                    Weight = 55,
                    ModifiedOn = DateTime.UtcNow,
                    Version = 0,
                };

                var n3 = new Node()
                {
                    Id = Guid.NewGuid().ToByteArray(),
                    ParentId = null,
                    DataId = d1.Id,
                    Word = Guid.NewGuid().ToByteArray(),
                    Weight = 55,
                    ModifiedOn = DateTime.UtcNow,
                    Version = 0,
                };

                await repo.InsertNode(n2);
                await repo.InsertNode(n3);

                var nx = await repo.GetNodeAncestry(n2.Id);
                Assert.AreEqual(2, nx.Count());

                ns1.Weight += 100;
                await repo.UpdateNode(ns1);

                var nss1 = await repo.GetNode(ns1.Id);
                Assert.AreEqual(1, nss1.Version);
                Assert.AreEqual(ns1.Version, nss1.Version);
                Assert.AreEqual(105, nss1.Weight);

                Assert.AreEqual(1, await repo.DeleteNode(n2.Id, n2.Version));
                Assert.AreEqual(1, await repo.DeleteNode(ns1.Id, ns1.Version));
                Assert.IsNull(await repo.GetNode(ns1.Id));

                Assert.AreEqual(1, await repo.DeleteNode(n3.Id, n3.Version));

                Assert.AreEqual(1, await repo.DeleteContent(ds1.Id));
                Assert.IsNull(await repo.GetContent(d1.Id));
            }
        }
    }
}
