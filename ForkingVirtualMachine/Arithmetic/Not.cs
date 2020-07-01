namespace ForkingVirtualMachine.Arithmetic
{
    public class Not : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Not();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopBool();

            execution.Context.Push(a
              ? Constants.False
              : Constants.True);
        }
    }
}
