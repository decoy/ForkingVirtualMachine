namespace ForkingVirtualMachine.Machines
{
    public class Multiply : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Multiply();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();
            context.Push(a * b);
        }
    }
}
