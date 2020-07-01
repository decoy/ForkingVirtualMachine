namespace ForkingVirtualMachine.Flow
{
    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(Execution execution)
        {
            // TODO: branch0 seems to be more common?
            if (!execution.Context.PopBool())
            {
                execution.Next();
            }
        }
    }
}
