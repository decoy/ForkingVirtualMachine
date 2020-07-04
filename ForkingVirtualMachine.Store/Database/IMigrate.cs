namespace ForkingVirtualMachine.Store.Database
{
    using System.Data.Common;
    using System.Threading.Tasks;

    public interface IMigrate
    {
        Task Up(DbConnection db);
    }
}
