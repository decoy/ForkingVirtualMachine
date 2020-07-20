﻿using ForkingVirtualMachine.Store.Database;
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
                Word = Guid.NewGuid().ToByteArray(),
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
                Assert.IsTrue(comparer.Equals(n1.Word, ns1.Word));
                Assert.AreEqual(n1.Weight, ns1.Weight);
                Assert.AreEqual(n1.ModifiedOn, ns1.ModifiedOn);
                Assert.AreEqual(n1.Version, ns1.Version);

                n1.Weight += 100;
                await repo.UpdateNode(n1);

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

                var child = await repo.GetChildNode(n1.Id, n2.Word);
                Assert.IsTrue(comparer.Equals(n2.Id, child.Id));

                var ancestors = await repo.GetNodeAncestry(n3.Id);
                Assert.AreEqual(2, ancestors.Count());
                Assert.IsTrue(comparer.Equals(n3.Id, ancestors.First().Id));
                Assert.IsTrue(comparer.Equals(n1.Id, ancestors.Skip(1).First().Id));
            }
        }
    }
}
