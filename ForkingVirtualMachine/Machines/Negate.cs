namespace ForkingVirtualMachine.Machines
{
    public class Negate : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Negate();

        public void Execute(IContext context)
        {
            var a = context.PopInt();
            context.Push(-a);
        }
    }
}
