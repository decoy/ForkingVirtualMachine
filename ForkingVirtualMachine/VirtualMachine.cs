namespace ForkingVirtualMachine
{
    public class VirtualMachine : IVirtualMachine
    {
        public void Execute(Context context)
        {
            var op = context.Execution.Next();
            var next = new Execution(context.Functions[op]);
            next.Machine.Execute(context);
        }
    }
}
