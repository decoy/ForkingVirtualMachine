namespace ForkingVirtualMachine.Arithmetic
{
    public class EqualTo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new EqualTo();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopInt();
            var b = execution.Context.PopInt();

            execution.Context.Push(a == b
                ? Constants.True
                : Constants.False);
        }
    }
}
