namespace ForkingVirtualMachine.Machines
{
    public class ContextMachine : IVirtualMachine
    {
        private readonly IVirtualMachine machine;

        public Context State { get; }

        public ContextMachine(IVirtualMachine machine, Context state)
        {
            this.machine = machine;
            State = state;
        }

        public void Execute(Context context)
        {
            var op = context.Execution.Current;
            if (State.Functions.ContainsKey(op))
            {
                var code = State.Functions[op].ToTrimmed(); // copy
                // local 'define' puts 0 at the beginning
                // only way to end up on that dictionary
                if (code.Current == 0)
                {
                    context.Execution.Next(); // eat the op
                    code.Next(); // eat the 0
                    context.Executions.Push(code);
                }
                else
                {
                    machine.Execute(context);
                }
            }
            else
            {
                throw new UnknownOperationException(op);
            }
        }
    }
}
