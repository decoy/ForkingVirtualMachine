namespace ForkingVirtualMachine.Arithmetic
{
    public class Negate : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Negate();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopInt();
            execution.Context.Push(-a);
        }
    }
}
