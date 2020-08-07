namespace ForkingVirtualMachine.Machines
{
    using System.Numerics;

    public class DivideRem : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new DivideRem();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            var res = BigInteger.DivRem(a, b, out var r);

            context.Push(r);
            context.Push(res);
        }
    }
}
