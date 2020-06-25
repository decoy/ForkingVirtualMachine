namespace ForkingVirtualMachine.Flow
{
    using System;

    public class Boom : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Boom();

        public void Execute(Context context)
        {
            throw new Exception();
        }
    }
}
