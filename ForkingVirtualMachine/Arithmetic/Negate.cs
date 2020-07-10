namespace ForkingVirtualMachine.Arithmetic
{
    public class Negate : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Negate();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            context.Push(-a);
        }
    }
}
