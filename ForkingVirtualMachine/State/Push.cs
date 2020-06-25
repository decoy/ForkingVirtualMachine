namespace ForkingVirtualMachine.State
{
    public class Push : IVirtualMachine
    {
        public void Execute(Context context)
        {
            context.Stack.Push(context.Execution.Next());
        }
    }
}
