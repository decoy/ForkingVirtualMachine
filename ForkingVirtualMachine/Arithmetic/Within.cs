namespace ForkingVirtualMachine.Arithmetic
{
    public class Within : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Within();

        public void Execute(Context context)
        {
            var x = context.Stack.Pop();
            var min = context.Stack.Pop();
            var max = context.Stack.Pop();
            if (x >= min && x < max)
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
