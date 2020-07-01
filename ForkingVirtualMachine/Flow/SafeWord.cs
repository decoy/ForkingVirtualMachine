namespace ForkingVirtualMachine.Flow
{
    public class SafeWord : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new SafeWord();

        public void Execute(Execution execution)
        {
            execution.Stop();
        }
    }
}
