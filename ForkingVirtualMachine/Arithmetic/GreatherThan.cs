namespace ForkingVirtualMachine.Math
{
    public class GreaterThan : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new GreaterThan();

        public void Execute(Context context)
        {
            if (context.Stack.Pop() > context.Stack.Pop())
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
