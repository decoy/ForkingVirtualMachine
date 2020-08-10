namespace ForkingVirtualMachine.Machines
{
    public class AsyncExecutable : IVirtualMachine
    {
        private readonly IAsyncVirtualMachine machine;

        public AsyncExecutable(IAsyncVirtualMachine machine)
        {
            this.machine = machine;
        }

        public void Execute(IScope scope, IContext context)
        {
            context.Push(new AsyncExecution(scope, machine));
        }
    }
}
