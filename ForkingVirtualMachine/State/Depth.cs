namespace ForkingVirtualMachine.State
{
    public class Depth : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Depth();

        public void Execute(Context context)
        {
            context.Stack.Push(context.Stack.Count);
        }
    }
}
