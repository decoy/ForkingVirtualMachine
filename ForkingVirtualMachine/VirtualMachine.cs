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
            // get the scope with each exe
            // really leaning towards this
            // but scopes are really just stacks... so.....
            // should context scope be part of the exe, not the other way around?

            // storing gets super weird...

            var op = context.Execution.Next();
            if (context.Execution.IsComplete)
            {
                context.Executions.Pop();
            }

            if (context.Scope == null && Machines.ContainsKey(op))
            {
                Machines[op].Execute(context);
            }
            else if (context.Scope != null && context.Scope.Functions.ContainsKey(op))
            {
                var code = context.Scope.Functions[op].Copy();
                if (code.Current != 0)
                {
                    context.Scope = context.Parent;
                }
                context.Executions.Push(code);
            }
            else
            {
                throw new UnknownOperationException(op);
            }
        }
    }
}
