namespace ForkingVirtualMachine.Math
{
    public class Add : IVirtualMachine
    {
        public void Execute(Context context)
        {
            context.Stack.Push(context.Stack.Pop() + context.Stack.Pop());
        }
    }
}
