namespace ForkingVirtualMachine.Flow
{
    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(Context context)
        {
            if (context.Stack.Pop() == 0)
            {
                context.Execution.Next();
            }
        }
    }
}
