namespace ForkingVirtualMachine.Machines
{
    using System.Threading.Tasks;

    public class AsyncExecution : IAsyncExecution
    {
        private readonly IScope scope;

        private readonly IAsyncVirtualMachine machine;

        public AsyncExecution(IScope scope, IAsyncVirtualMachine machine)
        {
            this.scope = scope;
            this.machine = machine;
        }

        public void Execute(IContext context)
        {
            machine.Execute(scope, context);
        }

        public Task ExecuteAsync(IContext context)
        {
            return machine.ExecuteAsync(scope, context);
        }
    }
}
