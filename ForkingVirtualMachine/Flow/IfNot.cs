namespace ForkingVirtualMachine.Flow
{
    public class IfNot : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new IfNot();

        public void Execute(Context context)
        {
            if (context.Stack.Pop() != 0)
            {
                context.Next();
            }
        }
    }
}
