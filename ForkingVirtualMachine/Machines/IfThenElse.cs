namespace ForkingVirtualMachine.Machines
{
    public class IfThenElse : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new IfThenElse();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.Pop();
            var b = context.Pop();

            if (context.PopBool())
            {
                context.Push(a);
            }
            else
            {
                context.Push(b);
            }
        }
    }
}
