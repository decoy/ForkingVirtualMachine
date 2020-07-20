namespace ForkingVirtualMachine.Store.Database
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Numerics;
    using System.Threading.Tasks;
    using Utility;

    // https://github.com/sqlkata/querybuilder neat
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
                name = reader.GetString(0),
                ran_on = reader.GetDateTime(1)
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
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE id = @id
                LIMIT 1;";

            return db.QuerySingle(sql, new { id }, ToNode);
        }

        public Task<IEnumerable<Node>> GetChildNodes(byte[] parentId, int limit = LIMIT)
        {
            const string sql = @"
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE parent_id = @parentId
                LIMIT @limit;";

            return db.Query(sql, new { parentId, limit }, ToNode);
        }

        public Task<Node> GetChildNode(byte[] parentId, byte[] word)
        {
            const string sql = @"
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE parent_id = @parentId
                AND word = @word
                LIMIT 1;";

            return db.QuerySingle(sql, new { parentId, word, }, ToNode);
        }

        public Task<IEnumerable<Node>> GetNodeAncestry(byte[] id)
        {
            const string sql = @"
                WITH RECURSIVE node_tree (id, parent_id, word, data_id, weight, modified_on, version) AS (
                    SELECT n1.id, n1.parent_id, n1.word, n1.data_id, n1.weight, n1.modified_on, n1.version
                    FROM nodes n1
                    WHERE n1.id = @id

                    UNION ALL

                    SELECT n2.id, n2.parent_id, n2.word, n2.data_id, n2.weight, n2.modified_on, n2.version
                    FROM nodes n2
                    JOIN node_tree ON n2.id = node_tree.parent_id
                )
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM node_tree;";

            return db.Query(sql, new { id }, ToNode);
        }

        public Task<int> InsertNode(Node node)
        {
            const string sql = @"
                INSERT INTO nodes 
                (id, parent_id, word, data_id, weight, modified_on, version)
                VALUES
                (@id, @parent_id, @word, @data_id, @weight, @modified_on, @version);";

            return db.Execute(sql, new
            {
                id = node.Id,
                parent_id = node.ParentId,
                word = node.Word,
                data_id = node.DataId,
                weight = node.Weight.ToByteArray(),
                modified_on = node.ModifiedOn,
                version = node.Version,
            });
        }

        public async Task UpdateNode(Node node)
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
                newversion = node.Version + 1,
                version = node.Version,
            });

            if (updates != 1)
            {
                throw new VersionUpdateException();
            }

            // TODO: don't like this modifying incoming.
            node.Version += 1;
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
                Word = (byte[])reader.GetValue(2),
                DataId = (byte[])reader.GetValue(3),
                Weight = new BigInteger((byte[])reader.GetValue(4)),
                ModifiedOn = reader.GetDateTime(5),
                Version = reader.GetInt32(6),
            };
        }

        #endregion
    }
}
