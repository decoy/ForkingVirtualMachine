namespace ForkingVirtualMachine.Math
{
    public class DivideRem : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new DivideRem();

        public void Execute(Context context)
        {
            var res = System.Math.DivRem(context.Stack.Pop(), context.Stack.Pop(), out var rem);
            context.Stack.Push(rem);
            context.Stack.Push(res);
        }
    }
}
