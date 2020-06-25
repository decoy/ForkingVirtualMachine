namespace ForkingVirtualMachine.State
{
    public class Push : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Push();

        public void Execute(Context context)
        {
            context.Stack.Push(context.Execution.Next());
        }
    }
}
