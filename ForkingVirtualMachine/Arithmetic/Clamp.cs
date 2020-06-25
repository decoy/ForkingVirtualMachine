namespace ForkingVirtualMachine.Arithmetic
{
    public class Clamp : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Clamp();

        public void Execute(Context context)
        {
            context.Stack.Push(System.Math.Clamp(
                context.Stack.Pop(),
                context.Stack.Pop(),
                context.Stack.Pop())
                );
        }
    }
}
