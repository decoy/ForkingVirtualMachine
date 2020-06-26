namespace ForkingVirtualMachine.Flow
{
    using System.Collections.Generic;

    public class Operation : IVirtualMachine
    {
        private readonly IVirtualMachine machine;

        public Operation(IVirtualMachine machine)
        {
            this.machine = machine;
        }

        public void Execute(Context context)
        {
            var scope = context.Execution.Scope;


            if (scope == null)
            {
                machine.Execute(context);
            }
            else
            {
                var op = context.Execution.Next();
                if (context.Execution.IsComplete)
                {
                    context.Executions.Pop();
                }

                if (scope.Functions.ContainsKey(op))
                {
                    context.Executions.Push(scope.Functions[op].Copy());
                }
                else
                {
                    throw new UnknownOperationException(scope, op);
                }
            }
        }
    }
}
