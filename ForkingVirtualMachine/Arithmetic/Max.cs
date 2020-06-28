using System.Numerics;

namespace ForkingVirtualMachine.Arithmetic
{
    public class Max : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Max();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            var b = context.Machine.LoadInt(context.Next());
            context.Machine.Store(context.Next(), BigInteger.Max(a, b));
        }
    }
}
