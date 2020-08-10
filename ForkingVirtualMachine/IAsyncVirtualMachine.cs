namespace ForkingVirtualMachine
{
    using System.Threading.Tasks;

    public interface IAsyncVirtualMachine : IVirtualMachine
    {
        public Task ExecuteAsync(IScope scope, IContext context);
    }
}
