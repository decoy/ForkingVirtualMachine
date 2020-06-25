namespace ForkingVirtualMachine.State
{
    public class Drop : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Drop();

        public void Execute(Context context)
        {
            context.Stack.Pop();
        }
    }
}
