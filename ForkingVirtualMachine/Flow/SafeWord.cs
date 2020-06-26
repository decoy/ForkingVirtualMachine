namespace ForkingVirtualMachine.Flow
{
    public class SafeWord : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new SafeWord();

        public void Execute(Context context)
        {
            throw new SafeWordException();
        }
    }
}
