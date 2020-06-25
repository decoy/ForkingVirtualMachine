namespace ForkingVirtualMachine.Math
{
    public class GreatherThanEqualTo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new GreatherThanEqualTo();

        public void Execute(Context context)
        {
            if (context.Stack.Pop() >= context.Stack.Pop())
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
