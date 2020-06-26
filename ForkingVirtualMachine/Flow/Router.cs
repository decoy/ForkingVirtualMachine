namespace ForkingVirtualMachine.Flow
{
    using System.Collections.Generic;

    public class Router : IVirtualMachine
    {
        public Dictionary<byte, IVirtualMachine> Machines { get; }

        public Router()
        {
            Machines = new Dictionary<byte, IVirtualMachine>();
        }

        public Router(Dictionary<byte, IVirtualMachine> machines)
        {
            Machines = machines;
        }

        public void Execute(Context context)
        {
            var op = context.Execution.Next();

            // is this necessary here?
            if (context.Execution.IsComplete)
            {
                context.Executions.Pop();
            }

            if (Machines.ContainsKey(op))
            {
                Machines[op].Execute(context);
            }
            else
            {
                throw new UnknownOperationException(context, op);
            }
        }
    }
}
