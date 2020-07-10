namespace ForkingVirtualMachine.Machines
{
    using System.Numerics;

    public class Min : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Min();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(BigInteger.Min(a, b));
        }
    }
}
