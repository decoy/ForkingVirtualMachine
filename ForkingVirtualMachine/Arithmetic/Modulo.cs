namespace ForkingVirtualMachine.Math
{
    public class Modulo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Modulo();

        public void Execute(Context context)
        {
            context.Stack.Push(checked(context.Stack.Pop() % context.Stack.Pop()));
        }
    }
}
