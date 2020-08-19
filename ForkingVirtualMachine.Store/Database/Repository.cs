namespace ForkingVirtualMachine.Store.Database
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Utility;

    public class Repository
    {
        private const int LIMIT = 100;

        private readonly DbConnection db;

        public Repository(DbConnection db)
        {
            this.db = db;
        }

        #region Migrations

        public Task<IEnumerable<Migration>> GetMigrations()
        {
            const string sql = @"
                SELECT name, ran_on 
                FROM migrations 
                ORDER BY name;";

            return db.Query(sql, ToMigration);
        }

        public Task<int> InsertMigration(string name, DateTime ranOn)
        {
            const string sql = @"
                INSERT INTO migrations 
                (name, ran_on)
                VALUES
                (@name, @ran_on);";

            return db.Execute(sql, new
            {
                name,
                ran_on = ranOn,
            });
        }

        public static Migration ToMigration(DbDataReader reader)
        {
            return new Migration()
            {
                Name = reader.GetString(0),
                RanOn = reader.GetDateTime(1)
            };
        }

        #endregion

        #region Contents

        public Task<int> InsertContent(Content content)
        {
            const string sql = @"
                INSERT INTO contents 
                (id, data)
                VALUES
                (@id, @data);";

            return db.Execute(sql, new
            {
                id = content.Id,
                data = content.Data,
            });
        }

        public Task<Content> GetContent(byte[] id)
        {
            const string sql = @"
                SELECT id, data
                FROM contents 
                WHERE id = @id
                LIMIT 1;";

            return db.QuerySingle(sql, new { id }, ToContent);
        }

        public Task<int> DeleteContent(byte[] id)
        {
            const string sql = @"
                DELETE FROM contents 
                WHERE id = @id;";

            return db.Execute(sql, new { id });
        }

        public static Content ToContent(DbDataReader reader)
        {
            return new Content()
            {
                Id = (byte[])reader.GetValue(0),
                Data = (byte[])reader.GetValue(1)
            };
        }

        #endregion

        #region Nodes

        public Task<Node> GetNode(byte[] id)
        {
            const string sql = @"
                SELECT id, from_id, to_id, data_id, sign, weight, modified_on, version
                FROM nodes 
                WHERE id = @id
                LIMIT 1;";

            return db.QuerySingle(sql, new { id }, ToNode);
        }

        public Task<IEnumerable<Node>> GetChildNodes(byte[] fromId, int limit = LIMIT)
        {
            const string sql = @"
                SELECT id, from_id, to_id, data_id, sign, weight, modified_on, version
                FROM nodes 
                WHERE from_id = @fromId
                LIMIT @limit;";

            return db.Query(sql, new { fromId, limit }, ToNode);
        }

        public Task<Node> GetChildNode(byte[] fromId, bool sign, byte[] toId)
        {
            const string sql = @"
                SELECT id, from_id, to_id, data_id, sign, weight, modified_on, version
                FROM nodes 
                WHERE from_id = @fromId
                AND to_id = @toId
                AND sign = @sign;";

            return db.QuerySingle(sql, new { fromId, toId, sign }, ToNode);
        }

        public Task<IEnumerable<Node>> GetNodeAncestry(byte[] id)
        {
            const string sql = @"
                WITH RECURSIVE node_tree (id, from_id, to_id, data_id, sign, weight, modified_on, version) AS (
                    SELECT n1.id, n1.from_id, n1.to_id, n1.data_id, n1.sign, n1.weight, n1.modified_on, n1.version
                    FROM nodes n1
                    WHERE n1.id = @id

                    UNION ALL

                    SELECT n2.id, n2.from_id, n2.to_id, n2.data_id, n2.sign, n2.weight, n2.modified_on, n2.version
                    FROM nodes n2
                    JOIN node_tree ON n2.id = node_tree.from_id
                )
                SELECT id, from_id, to_id, data_id, sign, weight, modified_on, version
                FROM node_tree;";

            return db.Query(sql, new { id }, ToNode);
        }

        public Task<int> InsertNode(Node node)
        {
            const string sql = @"
                INSERT INTO nodes 
                (id, from_id, to_id, data_id, sign, weight, modified_on, version)
                VALUES
                (@id, @from_id, @to_id, @data_id, @sign, @weight, @modified_on, @version);";

            return db.Execute(sql, new
            {
                id = node.Id,
                from_id = node.FromId,
                to_id = node.ToId,
                data_id = node.DataId,
                sign = node.Sign,
                weight = node.Weight.ToByteArray(),
                modified_on = node.ModifiedOn,
                version = node.Version,
            });
        }

        public async Task UpdateNode(Node node, int nextVersion)
        {
            const string sql = @"
                UPDATE nodes 
                SET 
                    weight = @weight,
                    modified_on = @modified_on,
                    version = @newversion
                WHERE id = @id 
                AND version = @version;";

            var updates = await db.Execute(sql, new
            {
                id = node.Id,
                weight = node.Weight.ToByteArray(),
                modified_on = node.ModifiedOn,
                newversion = nextVersion,
                version = node.Version,
            });

            if (updates != 1)
            {
                throw new VersionUpdateException();
            }

            node.Version = nextVersion;
        }

        public Task<int> DeleteNode(byte[] id, int version)
        {
            const string sql = @"
                DELETE FROM nodes 
                WHERE id = @id 
                AND version = @version;";

            return db.Execute(sql, new { id, version });
        }

        private static Node ToNode(DbDataReader reader)
        {
            return new Node()
            {
                Id = (byte[])reader.GetValue(0),
                FromId = reader.GetNullableValue<byte[]>(1),
                ToId = reader.GetNullableValue<byte[]>(2),
                DataId = (byte[])reader.GetValue(3),
                Sign = reader.GetBoolean(4),
                Weight = new BigInteger((byte[])reader.GetValue(5)),
                ModifiedOn = reader.GetDateTime(6),
                Version = reader.GetInt32(7),
            };
        }

        public static byte[] CreateNodeId(byte[] fromId, bool sign, byte[] toId, byte[] dataId)
        {
            using var sha = SHA256.Create();
            var id = new byte[fromId.Length + toId.Length + dataId.Length + 1];
            var i = 0;

            Buffer.BlockCopy(fromId, 0, id, i, fromId.Length);
            id[fromId.Length] = sign ? byte.MaxValue : byte.MinValue;
            i += fromId.Length + 1;

            Buffer.BlockCopy(toId, 0, id, i, toId.Length);
            i += toId.Length;

            Buffer.BlockCopy(dataId, 0, id, i, dataId.Length);

            return sha.ComputeHash(id);
        }

        #endregion

        #region exchange



        #endregion
    }
}
