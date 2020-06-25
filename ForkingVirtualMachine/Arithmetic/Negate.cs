namespace ForkingVirtualMachine.Arithmetic
{
    public class Negate : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Negate();

        public void Execute(Context context)
        {
            context.Stack.Push(-context.Stack.Pop());
        }
    }
}
