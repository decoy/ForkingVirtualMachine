namespace ForkingVirtualMachine.Math
{
    public class LessThan : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new LessThan();

        public void Execute(Context context)
        {
            if (context.Stack.Pop() < context.Stack.Pop())
            {
                context.Stack.Push(1);
            }
            else
            {
                context.Stack.Push(0);
            }
        }
    }
}
