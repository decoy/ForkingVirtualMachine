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
            const string sql = @"SELECT * FROM migrations ORDER BY name;";

            return await db.Query(sql, r => r.MapTo(new Migration()));
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

        #endregion

        #region Nodes

        public async Task<Node> GetNode(Guid id)
        {
            const string sql = @"
                SELECT * 
                FROM nodes 
                WHERE id = @id
                LIMIT 1;";

            return await db.QuerySingle(sql, new { id }, ToNode);
        }

        public async Task<IEnumerable<Node>> GetNodesByLabel(byte[] label, int limit = LIMIT)
        {
            const string sql = @"
                SELECT * 
                FROM nodes 
                WHERE label = @label
                LIMIT @limit;";

            return await db.Query(sql, new { label, limit }, ToNode);
        }

        public async Task<IEnumerable<Node>> GetNodesByParent(Guid parentId, int limit = LIMIT)
        {
            const string sql = @"
                SELECT * 
                FROM nodes 
                WHERE parent_id = @parent_id
                LIMIT @limit;";

            return await db.Query(sql, new { parent_id = parentId, limit }, ToNode);
        }

        public async Task<Node> GetChildNode(Guid parentId, byte[] label)
        {
            const string sql = @"
                SELECT * 
                FROM nodes 
                WHERE parent_id = @parentId
                AND label = @label
                LIMIT 1;";

            return await db.QuerySingle(sql, new
            {
                parentId,
                label,
            }, ToNode);
        }

        public async Task<Node> InsertNode(Node Node)
        {
            const string sql = @"
                INSERT INTO nodes 
                (id, parent_id, type, label, value, modified_on, version)
                VALUES
                (@id, @parent_id, @type, @label, @value, @modified_on, @version);";

            await db.Execute(sql, new
            {
                id = Node.Id,
                parent_id = Node.ParentId,
                type = Node.Type,
                label = Node.Label,
                value = Node.Weight.ToByteArray(),
                modified_on = Node.ModifiedOn,
                version = Node.Version,
            });

            return Node;
        }

        public async Task<Node> UpdateNode(Node Node)
        {
            const string sql = @"
                UPDATE nodes 
                SET 
                    value = @value,
                    modified_on = @modified_on,
                    version = @newversion
                WHERE id = @id 
                AND version = @version;";

            var updates = await db.Execute(sql, new
            {
                id = Node.Id,
                value = Node.Weight.ToByteArray(),
                modified_on = Node.ModifiedOn,
                newversion = Node.Version + 1,
                version = Node.Version,
            });

            if (updates != 1)
            {
                throw new VersionUpdateException();
            }

            // TODO: don't like this modifying incoming.
            Node.Version += 1;

            return Node;
        }

        public async Task<int> DeleteNode(Guid id, int version)
        {
            const string sql = @"
                DELETE FROM nodes 
                WHERE id = @id 
                AND version = @version;";

            return await db.Execute(sql, new { id, version });
        }

        private static Node ToNode(IDataReader reader)
        {
            return new Node()
            {
                Id = (Guid)reader["id"],
                ParentId = reader.GetValue<Guid?>("parent_id"),
                Type = (string)reader["type"],
                Label = (byte[])reader["label"],
                Weight = new BigInteger((byte[])reader["value"]), // nullable?
                ModifiedOn = (DateTime)reader["modified_on"],
                Version = (int)reader["version"],
            };
        }

        #endregion
    }
}
