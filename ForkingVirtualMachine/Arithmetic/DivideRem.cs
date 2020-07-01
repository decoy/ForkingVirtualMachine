namespace ForkingVirtualMachine.Arithmetic
{
    using System.Numerics;

    public class DivideRem : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new DivideRem();

        public void Execute(Execution execution)
        {
            var a = execution.Context.PopInt();
            var b = execution.Context.PopInt();

            var res = BigInteger.DivRem(a, b, out var r);

            execution.Context.Push(r);
            execution.Context.Push(res);
        }
    }
}
