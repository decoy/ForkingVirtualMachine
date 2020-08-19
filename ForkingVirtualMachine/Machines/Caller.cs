namespace ForkingVirtualMachine.Machines
{
    public class Caller : IVirtualMachine
    {
        public void Execute(IScope scope, IContext context)
        {
            // TODO: might not allow this?
            // instead getCallerId, etc.
            context.Push(scope.Caller);
        }
    }
}
