namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class VirtualMachine2 : IVirtualMachine
    {
        public Dictionary<byte, IVirtualMachine> Machines { get; }

        public VirtualMachine2()
        {
            Machines = new Dictionary<byte, IVirtualMachine>();
        }

        public VirtualMachine2(Dictionary<byte, IVirtualMachine> machines)
        {
            Machines = machines;
        }

        public void Execute(Context context)
        {
            var op = context.Execution.Next();

            if (context.Scope == null && Machines.ContainsKey(op))
            {
                Machines[op].Execute(context);
            }
            else if (context.Scope.Functions.ContainsKey(op))
            {
                // get a scoped copy of the bytes
                // HOW DO WE POP THE SCOPE?
                // HOW DO WE ROUTE THE EXE?

                // put the scope _in the exe_?
                // pop on empty? arg.
                var code = context.Scope.Functions[op].ToScope(context.Execution.Scope);
                
                context.Executions.Push(code);
            }
            else
            {
                throw new UnknownOperationException(op);
            }
        }
    }
}
