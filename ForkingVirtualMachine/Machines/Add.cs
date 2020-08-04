namespace ForkingVirtualMachine.Machines
{
    public class Add : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Add();

        public void Execute(IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();
            context.Push(a + b);
        }
    }
}
