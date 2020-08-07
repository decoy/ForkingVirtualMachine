namespace ForkingVirtualMachine.Machines
{
    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

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
