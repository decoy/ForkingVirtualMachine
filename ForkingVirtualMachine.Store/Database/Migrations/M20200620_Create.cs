namespace ForkingVirtualMachine.Store.Database.Migrations
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Utility;

    public class M20200620_Create : IMigrate
    {
        public static async Task CreateNodesTable(DbConnection db)
        {
            // CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
            // SELECT uuid_generate_v1();
            //id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
            const string sql = @"
                CREATE TABLE nodes (
                    id UUID PRIMARY KEY,                
                    parent_id UUID,
                    type VARCHAR(25) NOT NULL,
                    label BYTEA NOT NULL,
                    value BYTEA,
                    version INTEGER NOT NULL,
                    modified_on TIMESTAMP NOT NULL
                );";

            //TODO indexes

            await db.Execute(sql);
        }

        public static async Task CreateNodeHistoryTable(DbConnection db)
        {
            // THE FEED

            const string sql = @"
                CREATE TABLE node_history (
                    id UUID PRIMARY KEY,                
                    parent_id UUID,
                    type VARCHAR(25) NOT NULL,
                    label BYTEA NOT NULL,
                    value BYTEA,
                    version INTEGER NOT NULL,
                    modified_on TIMESTAMP NOT NULL
                );";

            await db.Execute(sql);
        }

        public async Task Up(DbConnection db)
        {
            using (var trans = db.BeginTransaction())
            {
                await CreateNodesTable(db);
                trans.Commit();
            }
        }
    }
}
