namespace ForkingVirtualMachine.Arithmetic
{
    public class Subtract : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Subtract();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopInt();
            var b = execution.Context.PopInt();
            execution.Context.Push(a - b);
        }
    }
}
