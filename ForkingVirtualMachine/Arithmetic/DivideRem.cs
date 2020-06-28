namespace ForkingVirtualMachine.Arithmetic
{
    using System.Numerics;

    public class DivideRem : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new DivideRem();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            var b = context.Machine.LoadInt(context.Next());

            var res = BigInteger.DivRem(a, b, out var r);

            context.Machine.Store(context.Next(), res);
            context.Machine.Store(context.Next(), r);
        }
    }
}
