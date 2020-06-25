namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class VirtualMachine : IVirtualMachine
    {
        public Dictionary<byte, IVirtualMachine> Machines { get; }

        public VirtualMachine()
        {
            Machines = new Dictionary<byte, IVirtualMachine>();
        }

        public VirtualMachine(Dictionary<byte, IVirtualMachine> machines)
        {
            Machines = machines;
        }

        public void Execute(Context context)
        {
            // we could grab a bigger index value here
            var op = context.Execution.Next();
            if (Machines.ContainsKey(op))
            {
                Machines[op].Execute(context);
            }
            else
            {
                throw new UnknownOperationException(op);
            }
        }
    }
}
