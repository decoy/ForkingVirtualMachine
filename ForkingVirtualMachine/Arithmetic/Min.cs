namespace ForkingVirtualMachine.Math
{
    public class Min : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Min();

        public void Execute(Context context)
        {
            context.Stack.Push(System.Math.Min(context.Stack.Pop(), context.Stack.Pop()));
        }
    }
}
