namespace ForkingVirtualMachine.Machines
{
    public class Caller : IVirtualMachine
    {
        public void Execute(IContext context)
        {
            context.Caller.Execute(context);
        }
    }
}
