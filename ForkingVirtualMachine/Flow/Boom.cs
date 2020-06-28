namespace ForkingVirtualMachine.Flow
{
    public class Boom : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Boom();

        public void Execute(Context context)
        {
            throw new BoundaryException();
        }
    }
}
