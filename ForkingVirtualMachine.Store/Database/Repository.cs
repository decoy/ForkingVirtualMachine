namespace ForkingVirtualMachine.Store.Database
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Numerics;
    using System.Reflection.Emit;
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

        public async Task<IEnumerable<Migration>> GetMigrations()
        {
            const string sql = @"SELECT name, ran_on FROM migrations ORDER BY name;";

            return await db.Query(sql, ToMigration);
        }

        public async Task<int> InsertMigration(string name, DateTime ranOn)
        {
            const string sql = @"
                INSERT INTO migrations 
                (name, ran_on)
                VALUES
                (@name, @ran_on);";

            return await db.Execute(sql, new
            {
                name,
                ran_on = ranOn,
            });
        }

        public static Migration ToMigration(IDataReader reader)
        {
            return new Migration()
            {
                name = reader.GetString(0),
                ran_on = reader.GetDateTime(1)
            };
        }

        #endregion

        #region Nodes

        public async Task<Node> GetNode(byte[] id)
        {
            const string sql = @"
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE id = @id
                LIMIT 1;";

            return await db.QuerySingle(sql, new { id }, ToNode);
        }

        public async Task<IEnumerable<Node>> GetNodesByLabel(byte[] word, int limit = LIMIT)
        {
            const string sql = @"
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE word = @word
                LIMIT @limit;";

            return await db.Query(sql, new { word, limit }, ToNode);
        }

        public async Task<IEnumerable<Node>> GetNodesByParent(byte[] parentId, int limit = LIMIT)
        {
            const string sql = @"
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE parent_id = @parentId
                LIMIT @limit;";

            return await db.Query(sql, new { parentId, limit }, ToNode);
        }

        public async Task<Node> GetChildNode(byte[] parentId, byte[] word)
        {
            const string sql = @"
                SELECT id, parent_id, word, data_id, weight, modified_on, version
                FROM nodes 
                WHERE parent_id = @parentId
                AND word = @word
                LIMIT 1;";

            return await db.QuerySingle(sql, new { parentId, word, }, ToNode);
        }

        public async Task<Node> InsertNode(Node node)
        {
            const string sql = @"
                INSERT INTO nodes 
                (id, parent_id, word, data_id, weight, modified_on, version)
                VALUES
                (@id, @parent_id, @word, @data_id, @weight, @modified_on, @version);";

            await db.Execute(sql, new
            {
                id = node.Id,
                parent_id = node.ParentId,
                word = node.Word,
                data_id = node.DataId,
                weight = node.Weight.ToByteArray(),
                modified_on = node.ModifiedOn,
                version = node.Version,
            });

            return node;
        }

        public async Task<Node> UpdateNode(Node node)
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

            return node;
        }

        public async Task<int> DeleteNode(byte[] id, int version)
        {
            const string sql = @"
                DELETE FROM nodes 
                WHERE id = @id 
                AND version = @version;";

            return await db.Execute(sql, new { id, version });
        }

        private static Node ToNode(IDataReader reader)
        {
            // id, parent_id, word, data_id, weight, modified_on, version
            return new Node()
            {
                Id = (byte[])reader.GetValue(0),
                ParentId = (byte[])reader.GetValue(1),
                Word = (byte[])reader.GetValue(2),
                DataId = (byte[])reader.GetValue(3),
                Weight = new BigInteger((byte[])reader.GetValue(4)),
                ModifiedOn = reader.GetDateTime(5),
                Version = reader.GetInt32(6),
            };

            //return new Node()
            //{
            //    Id = (byte[])reader["id"],
            //    ParentId = (byte[])reader["parent_id"],
            //    Word = (byte[])reader["word"],
            //    DataId = (byte[])reader["data_id"],
            //    Weight = new BigInteger((byte[])reader["weight"]),
            //    ModifiedOn = (DateTime)reader["modified_on"],
            //    Version = (int)reader["version"],
            //};
        }

        #endregion
    }
}
