namespace ForkingVirtualMachine.Flow
{
    using System.Collections.Generic;
    using System.Numerics;

    public class Load : IVirtualMachine
    {
        private Dictionary<BigInteger, IVirtualMachine> machines;

        public Load(Dictionary<BigInteger, IVirtualMachine> machines)
        {
            this.machines = machines;
        }

        public void Execute(Context context)
        {
            var word = new BigInteger(context.Pop().Span);
            if (machines.ContainsKey(word))
            {
                machines[word].Execute(context);
            }
        }
    }
}
