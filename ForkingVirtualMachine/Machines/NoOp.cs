namespace ForkingVirtualMachine.Machines
{
    public class NoOp : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new NoOp();

        public void Execute(Context context) { }
    }
}
