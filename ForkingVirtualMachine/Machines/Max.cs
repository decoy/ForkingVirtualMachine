using System.Numerics;

namespace ForkingVirtualMachine.Machines
{
    public class Max : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Max();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(BigInteger.Max(a, b));
        }
    }
}
