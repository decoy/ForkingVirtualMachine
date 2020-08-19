namespace ForkingVirtualMachine.Machines
{
    public class Caller : IVirtualMachine
    {
        public void Execute(IScope scope, IContext context)
        {
            context.Push(scope.Caller);
        }
    }
}
