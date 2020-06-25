namespace ForkingVirtualMachine.Math
{
    public class And : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new And();

        public void Execute(Context context)
        {
            var a = context.Stack.Pop();
            var b = context.Stack.Pop();
            if ((a != 0) && (b != 0))
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
