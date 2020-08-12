using ForkingVirtualMachine.Store.Database;
using ForkingVirtualMachine.Store.Database.Migrations;
using ForkingVirtualMachine.Store.Models;
using ForkingVirtualMachine.Utility;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
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

        private static Content CreateContent()
        {
            return new Content()
            {
                Id = Guid.NewGuid().ToByteArray(),
                Data = Guid.NewGuid().ToByteArray(),
            };
        }

        private static Node CreateNode(byte[] dataId, byte[] parentId = null)
        {
            return new Node()
            {
                Id = Guid.NewGuid().ToByteArray(),
                ParentId = parentId,
                DataId = dataId,
                Sign = true,
                Weight = 5,
                ModifiedOn = DateTime.UtcNow,
                Version = 0,
            };
        }

        [TestMethod]
        public async Task Migrates()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);
                var ran = (await repo.GetMigrations()).ToList();

                Assert.AreEqual(1, ran.Count);
                Assert.AreEqual(typeof(M20200620_Create).Name, ran[0].Name);
            }
        }

        [TestMethod]
        public async Task ContentsCRDTests()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);

                var d1 = CreateContent();

                Assert.AreEqual(1, await repo.InsertContent(d1));

                var ds1 = await repo.GetContent(d1.Id);

                Assert.IsTrue(comparer.Equals(d1.Id, ds1.Id));
                Assert.IsTrue(comparer.Equals(d1.Data, ds1.Data));

                Assert.AreEqual(1, await repo.DeleteContent(d1.Id));
                Assert.IsNull(await repo.GetContent(d1.Id));
            }
        }

        [TestMethod]
        public async Task NodesCRUDTests()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);
                var d1 = CreateContent();
                await repo.InsertContent(d1);

                var n1 = CreateNode(d1.Id);

                Assert.AreEqual(1, await repo.InsertNode(n1));

                var ns1 = await repo.GetNode(n1.Id);

                Assert.IsTrue(comparer.Equals(n1.Id, ns1.Id));
                Assert.IsTrue(comparer.Equals(n1.ParentId, ns1.ParentId));
                Assert.IsTrue(comparer.Equals(n1.DataId, ns1.DataId));
                Assert.AreEqual(n1.Weight, ns1.Weight);
                Assert.AreEqual(n1.ModifiedOn, ns1.ModifiedOn);
                Assert.AreEqual(n1.Version, ns1.Version);

                n1.Weight += 100;
                await repo.UpdateNode(n1, n1.Version + 1);

                var nsu1 = await repo.GetNode(n1.Id);

                Assert.AreEqual(1, nsu1.Version);
                Assert.AreEqual(n1.Version, nsu1.Version);
                Assert.AreEqual(105, nsu1.Weight);

                Assert.AreEqual(1, await repo.DeleteNode(nsu1.Id, nsu1.Version));
                Assert.IsNull(await repo.GetNode(nsu1.Id));
            }
        }

        [TestMethod]
        public async Task NodesLoadsChildren()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);

                var d1 = CreateContent();
                await repo.InsertContent(d1);

                var n1 = CreateNode(d1.Id);
                await repo.InsertNode(n1);

                var n2 = CreateNode(d1.Id, n1.Id);
                await repo.InsertNode(n2);

                var n3 = CreateNode(d1.Id, n1.Id);
                await repo.InsertNode(n3);

                var n4 = CreateNode(d1.Id);
                await repo.InsertNode(n4);

                var children = await repo.GetChildNodes(n1.Id);
                Assert.AreEqual(2, children.Count());

                var ancestors = await repo.GetNodeAncestry(n3.Id);
                Assert.AreEqual(2, ancestors.Count());
                Assert.IsTrue(comparer.Equals(n3.Id, ancestors.First().Id));
                Assert.IsTrue(comparer.Equals(n1.Id, ancestors.Skip(1).First().Id));
            }
        }

        [TestMethod]
        public async Task GetsChildNode()
        {
            using (var db = await OpenDb())
            {
                var repo = new Repository(db);

                var d1 = CreateContent();
                await repo.InsertContent(d1);

                var d2 = CreateContent();
                await repo.InsertContent(d2);

                var n1 = CreateNode(d1.Id);
                await repo.InsertNode(n1);

                var n2 = CreateNode(d1.Id, n1.Id);
                await repo.InsertNode(n2);

                var n3 = CreateNode(d2.Id, n1.Id);
                await repo.InsertNode(n3);

                var child = await repo.GetChildNode(n1.Id, true, d2.Id);
                Assert.IsTrue(comparer.Equals(n3.Id, child.Id));
            }
        }

        [TestMethod]
        public void CreatesNodeId()
        {
            using var sha = SHA256.Create();
            var from = new byte[] { 1, 2, 3 };
            var to = new byte[] { 6, 7, 8 };

            var id = Repository.CreateNodeId(from, true, to);

            var ex = sha.ComputeHash(new byte[] { 1, 2, 3, 255, 6, 7, 8 });
            Assert.IsTrue(comparer.Equals(ex, id));


            var id2 = Repository.CreateNodeId(from, false, to);

            var ex2 = sha.ComputeHash(new byte[] { 1, 2, 3, 0, 6, 7, 8 });
            Assert.IsTrue(comparer.Equals(ex2, id2));
        }
    }
}
