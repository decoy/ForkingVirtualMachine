namespace ForkingVirtualMachine.Machines
{
    using System.Collections.Generic;
    using System.Numerics;

    public class Define : IVirtualMachine
    {
        private Dictionary<BigInteger, IVirtualMachine> machines;

        private readonly IVirtualMachine scope;

        public Define(IVirtualMachine scope, Dictionary<BigInteger, IVirtualMachine> machines)
        {
            this.machines = machines;
            this.scope = scope;
        }

        public void Execute(Context context)
        {
            var word = new BigInteger(context.Pop().Span);
            var data = context.Pop();
            var exe = new Executable(scope, data);
            if (!machines.ContainsKey(word))
            {
                machines.Add(word, exe);
            }
            else
            {
                machines[word] = exe;
            }
        }
    }
}
