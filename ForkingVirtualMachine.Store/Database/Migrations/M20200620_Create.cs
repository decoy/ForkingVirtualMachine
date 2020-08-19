namespace ForkingVirtualMachine.Store.Database.Migrations
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Utility;

    public class M20200620_Create : IMigrate
    {
        public static Task CreateNodesTable(DbConnection db)
        {
            const string sql = @"
                CREATE TABLE nodes (
                    id          BLOB     PRIMARY KEY
                                         UNIQUE
                                         NOT NULL,
                    from_id     BLOB     REFERENCES nodes (id),
                    to_id       BLOB     REFERENCES nodes (id),
                    data_id     BLOB     NOT NULL
                                         REFERENCES contents (id),
                    sign        BOOLEAN  NOT NULL,
                    weight      BLOB     NOT NULL,                    
                    modified_on DATETIME NOT NULL,
                    version     INTEGER  NOT NULL
                );";

            return db.Execute(sql);
        }

        public static Task CreateContentsTable(DbConnection db)
        {
            const string sql = @"
                CREATE TABLE contents (
                    id   BLOB PRIMARY KEY
                              UNIQUE,
                    data BLOB NOT NULL
                );";

            return db.Execute(sql);
        }

        public async Task Up(DbConnection db)
        {
            using (var trans = db.BeginTransaction())
            {
                await CreateContentsTable(db);
                await CreateNodesTable(db);
                trans.Commit();
            }
        }
    }
}
