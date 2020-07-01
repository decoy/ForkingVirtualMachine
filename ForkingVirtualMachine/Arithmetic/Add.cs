namespace ForkingVirtualMachine.Arithmetic
{
    public class Add : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Add();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopInt();
            var b = execution.Context.PopInt();
            execution.Context.Push(a + b);
        }
    }
}
