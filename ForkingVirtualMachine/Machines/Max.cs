using System.Numerics;

namespace ForkingVirtualMachine.Machines
{
    public class Max : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Max();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(BigInteger.Max(a, b));
        }
    }
}
