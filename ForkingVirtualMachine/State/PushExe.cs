namespace ForkingVirtualMachine.State
{
    public class PushExe : IVirtualMachine
    {
        public void Execute(Context context)
        {
            var n = context.Execution.Next();
            var code = context.Execution.Next(n);
            context.Executions.Push(new Execution(code.ToArray()));
        }
    }
}
