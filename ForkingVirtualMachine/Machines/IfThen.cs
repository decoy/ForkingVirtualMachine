namespace ForkingVirtualMachine.Machines
{
    public class IfThen : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new IfThen();

        public void Execute(IScope scope, IContext context)
        {
            var then = context.Pop();

            if (context.PopBool())
            {
                context.Push(then);
            }
        }
    }
}
