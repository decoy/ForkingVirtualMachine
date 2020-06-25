namespace ForkingVirtualMachine.State
{
    public class PushExe : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new PushExe();

        public void Execute(Context context)
        {
            var n = context.Execution.Next();
            var code = context.Execution.Next(n);
            context.Executions.Push(new Execution(context, code.ToArray()));
        }
    }
}
