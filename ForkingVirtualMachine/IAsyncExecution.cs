namespace ForkingVirtualMachine
{
    using System.Threading.Tasks;

    public interface IAsyncExecution : IExecution
    {
        public Task ExecuteAsync(IContext context);
    }
}
