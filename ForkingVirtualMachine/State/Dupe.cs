namespace ForkingVirtualMachine.State
{
    public class Dupe : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Dupe();

        public void Execute(Context context)
        {
            var data = context.Stack.Pop();
            context.Stack.Push(data);
            context.Stack.Push(data);
        }
    }
}
