namespace ForkingVirtualMachine.Flow
{
    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(Context context)
        {
            // TODO: branch0 seems to be more common?
            // ... wait.
            // how do you use if?
            if (!context.PopBool())
            {
                //context.Next();
            }
        }
    }
}
