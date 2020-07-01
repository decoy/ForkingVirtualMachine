namespace ForkingVirtualMachine.Arithmetic
{
    public class Or : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Or();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopBool();
            var b = execution.Context.PopBool();
            execution.Context.Push((a || b)
                ? Constants.True
                : Constants.False);
        }
    }
}
