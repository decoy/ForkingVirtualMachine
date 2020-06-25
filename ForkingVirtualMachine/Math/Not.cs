namespace ForkingVirtualMachine.Math
{
    public class Not : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Not();

        public void Execute(Context context)
        {
            if (context.Stack.Pop() == 0)
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
