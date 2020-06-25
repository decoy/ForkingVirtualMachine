namespace ForkingVirtualMachine.Arithmetic
{
    public class Divide : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Divide();

        public void Execute(Context context)
        {
            context.Stack.Push(checked(context.Stack.Pop() / context.Stack.Pop()));
        }
    }
}
