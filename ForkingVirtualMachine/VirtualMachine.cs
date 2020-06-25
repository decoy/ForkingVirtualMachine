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

        public void Run(Context context)
        {
            // just a simple runner for now
            while (context.Executions.Count > 0)
            {
                Execute(context);
            }
        }

        public void Execute(Context context)
        {
            var scope = context.Execution.Scope;
            var op = context.Execution.Next();

            if (context.Execution.IsComplete)
            {
                context.Executions.Pop();
            }

            if (scope == null && Machines.ContainsKey(op))
            {
                Machines[op].Execute(context);
            }
            else if (scope != null && scope.Functions.ContainsKey(op))
            {
                context.Executions.Push(scope.Functions[op].Copy());
            }
            else
            {
                throw new UnknownOperationException(op);
            }
        }
    }
}
