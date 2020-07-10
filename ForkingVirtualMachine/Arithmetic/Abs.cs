using System.Numerics;

namespace ForkingVirtualMachine.Arithmetic
{
    public class Abs : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Abs();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            context.Push(BigInteger.Abs(a));
        }
    }
}
