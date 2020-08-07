namespace ForkingVirtualMachine.Machines
{
    public class Not : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Not();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopBool();

            context.Push(!a);
        }
    }
}
