namespace ForkingVirtualMachine.Arithmetic
{
    public class Divide : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Divide();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();
            context.Push(a / b);
        }
    }
}
