namespace ForkingVirtualMachine.Machines
{
    using System.Numerics;

    public class Min : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Min();

        public void Execute(IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(BigInteger.Min(a, b));
        }
    }
}
