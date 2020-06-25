namespace ForkingVirtualMachine.Math
{
    public class Multiply : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Multiply();

        public void Execute(Context context)
        {
            context.Stack.Push(checked(context.Stack.Pop() * context.Stack.Pop()));
        }
    }
}
