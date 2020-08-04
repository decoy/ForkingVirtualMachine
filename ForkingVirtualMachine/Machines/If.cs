namespace ForkingVirtualMachine.Machines
{
    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(IContext context)
        {
            if (!context.PopBool())
            {
                context.Pop(); // nom
            }
        }
    }
}
