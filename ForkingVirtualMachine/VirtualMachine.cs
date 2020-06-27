namespace ForkingVirtualMachine
{
    public class VirtualMachine : IVirtualMachine
    {
        public void Execute(Context context)
        {
            var op = context.Next();
            context.Functions[op].Machine.Execute(context);
        }
    }
}
