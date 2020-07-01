namespace ForkingVirtualMachine.Arithmetic
{
    public class Multiply : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Multiply();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopInt();
            var b = execution.Context.PopInt();
            execution.Context.Push(a * b);
        }
    }
}
