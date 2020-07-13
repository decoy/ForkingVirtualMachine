namespace ForkingVirtualMachine.Machines
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class Define : IVirtualMachine, IDescribe
    {
        private Dictionary<BigInteger, IVirtualMachine> machines;

        public Define(Dictionary<BigInteger, IVirtualMachine> machines)
        {
            this.machines = machines;
        }

        public void Execute(Context context)
        {
            var word = new BigInteger(context.Pop().Span);
            var data = context.Pop();

            if (data.Length == 0)
            {
                machines.Remove(word);
                return;
            }

            var exe = new Executable(this, data);
            if (!machines.ContainsKey(word))
            {
                machines.Add(word, exe);
            }
            else
            {
                machines[word] = exe;
            }
        }

        public IVirtualMachine Describe(ReadOnlyMemory<byte> word)
        {
            var key = new BigInteger(word.Span);
            if (machines.ContainsKey(key))
            {
                return machines[key];
            }

            return NoOp.Machine;
        }
    }
}
