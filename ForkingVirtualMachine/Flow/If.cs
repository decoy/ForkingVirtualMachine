namespace ForkingVirtualMachine.Flow
{
    using Arithmetic;

    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(Context context)
        {
            if (context.Machine.LoadBool(context.Next()))
            {
                context.Next();
            }
        }
    }
}
