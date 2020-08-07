namespace ForkingVirtualMachine.Machines
{
    public class LessThan : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new LessThan();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a < b);
        }
    }
}
