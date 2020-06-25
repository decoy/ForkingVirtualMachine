namespace ForkingVirtualMachine.Machines
{
    using ForkingVirtualMachine.Extensions;

    public class ContextMachine : IVirtualMachine
    {
        private readonly IVirtualMachine machine;

        public ContextMachine(IVirtualMachine machine)
        {
            this.machine = machine;
        }

        public void Execute(Context context)
        {
            var scope = context.GetExecutionScope();

            if (scope == null)
            {
                machine.Execute(context);
            }
            else
            {
                var op = context.Execution.Next();
                if (scope.Functions.ContainsKey(op))
                {
                    // get a scoped copy
                    var code = scope.Functions[op].ToScope(context.Execution.Scope);
                    context.Executions.Push(code);
                }
                else
                {
                    throw new UnknownOperationException(op);
                }
            }
        }
    }
}
