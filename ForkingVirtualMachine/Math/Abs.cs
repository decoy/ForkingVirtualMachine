namespace ForkingVirtualMachine.Math
{
    public class Abs : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Abs();

        public void Execute(Context context)
        {
            context.Stack.Push(System.Math.Abs(context.Stack.Pop()));
        }
    }
}
