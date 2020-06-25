namespace ForkingVirtualMachine.State
{
    public class Swap : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Swap();

        public void Execute(Context context)
        {
            var a = context.Stack.Pop();
            var b = context.Stack.Pop();
            context.Stack.Push(a);
            context.Stack.Push(b);
        }
    }
}
