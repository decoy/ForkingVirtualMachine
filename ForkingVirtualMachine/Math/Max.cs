namespace ForkingVirtualMachine.Math
{
    public class Max : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Max();

        public void Execute(Context context)
        {
            context.Stack.Push(System.Math.Max(context.Stack.Pop(), context.Stack.Pop()));
        }
    }
}
