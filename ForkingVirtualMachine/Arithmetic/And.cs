namespace ForkingVirtualMachine.Arithmetic
{
    public class And : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new And();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopBool();
            var b = execution.Context.PopBool();
            execution.Context.Push((a && b) 
                ? Constants.True 
                : Constants.False);
        }
    }
}
