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
                SELECT id, parent_id, data_id, sign, weight, modified_on, version
                FROM nodes 
                WHERE id = @id
                LIMIT 1;";

            return db.QuerySingle(sql, new { id }, ToNode);
        }

        public Task<IEnumerable<Node>> GetChildNodes(byte[] parentId, int limit = LIMIT)
        {
            const string sql = @"
                SELECT id, parent_id, data_id, sign, weight, modified_on, version
                FROM nodes 
                WHERE parent_id = @parentId
                LIMIT @limit;";

            return db.Query(sql, new { parentId, limit }, ToNode);
        }

        public Task<Node> GetChildNode(byte[] parentId, bool sign, byte[] toId)
        {
            const string sql = @"
                SELECT id, parent_id, data_id, sign, weight, modified_on, version
                FROM nodes 
                WHERE parent_id = @parentId
                AND data_id = @toId
                AND sign = @sign;";

            return db.QuerySingle(sql, new { parentId, toId, sign }, ToNode);
        }

        public Task<IEnumerable<Node>> GetNodeAncestry(byte[] id)
        {
            const string sql = @"
                WITH RECURSIVE node_tree (id, parent_id, data_id, sign, weight, modified_on, version) AS (
                    SELECT n1.id, n1.parent_id, n1.data_id, n1.sign, n1.weight, n1.modified_on, n1.version
                    FROM nodes n1
                    WHERE n1.id = @id

                    UNION ALL

                    SELECT n2.id, n2.parent_id, n2.data_id, n2.sign, n2.weight, n2.modified_on, n2.version
                    FROM nodes n2
                    JOIN node_tree ON n2.id = node_tree.parent_id
                )
                SELECT id, parent_id, data_id, sign, weight, modified_on, version
                FROM node_tree;";

            return db.Query(sql, new { id }, ToNode);
        }

        public Task<int> InsertNode(Node node)
        {
            const string sql = @"
                INSERT INTO nodes 
                (id, parent_id, data_id, sign, weight, modified_on, version)
                VALUES
                (@id, @parent_id, @data_id, @sign, @weight, @modified_on, @version);";

            return db.Execute(sql, new
            {
                id = node.Id,
                parent_id = node.ParentId,
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
                ParentId = reader.GetNullableValue<byte[]>(1),
                DataId = (byte[])reader.GetValue(2),
                Sign = reader.GetBoolean(3),
                Weight = new BigInteger((byte[])reader.GetValue(4)),
                ModifiedOn = reader.GetDateTime(5),
                Version = reader.GetInt32(6),
            };
        }

        public static byte[] CreateNodeId(byte[] parentId, bool sign, byte[] toId)
        {
            using var sha = SHA256.Create();
            var id = new byte[parentId.Length + toId.Length + 1];
            Buffer.BlockCopy(parentId, 0, id, 0, parentId.Length);
            Buffer.BlockCopy(toId, 0, id, parentId.Length + 1, toId.Length);
            id[parentId.Length] = sign ? byte.MaxValue : byte.MinValue;
            return sha.ComputeHash(id);
        }

        #endregion

        #region exchange

        public async Task<Node> GetOrCreateChild(byte[] nodeId, bool sign, byte[] toId)
        {
            var to = await GetChildNode(nodeId, sign, toId);
            if (to == null)
            {
                var id = CreateNodeId(nodeId, sign, toId);
                to = new Node()
                {
                    Id = id,
                    DataId = toId,
                    Sign = sign,
                    ParentId = nodeId,
                    Version = 0,
                    Weight = 0,
                };

                await InsertNode(to);
            }

            return to;
        }

        public async Task Transfer(byte[] fromId, byte[] toId, BigInteger delta, DateTime time)
        {
            if (delta <= 0)
            {
                throw new BoundaryException();
            }

            var from = await GetNode(fromId);

            if (from.Weight < delta)
            {
                throw new BoundaryException();
            }

            from.Weight -= delta;
            from.ModifiedOn = time;

            var to = await GetOrCreateChild(from.ParentId, from.Sign, toId);
            to.Weight += delta;
            to.ModifiedOn = time;

            await UpdateNode(from, from.Version + 1);
            await UpdateNode(to, to.Version + 1);
        }

        #endregion
    }
}
