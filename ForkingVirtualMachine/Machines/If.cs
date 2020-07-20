namespace ForkingVirtualMachine.Machines
{
    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(Context context)
        {
            if (!context.PopBool())
            {
                context.Pop(); // nom
            }
        }
    }
}
