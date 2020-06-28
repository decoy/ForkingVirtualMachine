namespace ForkingVirtualMachine.Arithmetic
{
    using System.Numerics;

    public class Min : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Min();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            var b = context.Machine.LoadInt(context.Next());
            context.Machine.Store(context.Next(), BigInteger.Max(a, b));
        }
    }
}
