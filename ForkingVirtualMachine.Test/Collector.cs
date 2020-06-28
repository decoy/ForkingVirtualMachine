using ForkingVirtualMachine.Arithmetic;
using System.Collections.Generic;
using System.Numerics;

namespace ForkingVirtualMachine.Test
{
    public class Collector : IVirtualMachine
    {
        public Queue<BigInteger> Collected { get; } = new Queue<BigInteger>();

        public void Execute(Context context)
        {
            var word = context.Next();
            Collected.Enqueue(context.Machine.LoadInt(word));
        }
    }
}
