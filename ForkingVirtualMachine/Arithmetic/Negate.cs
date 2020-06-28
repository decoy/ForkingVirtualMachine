namespace ForkingVirtualMachine.Arithmetic
{
    public class Negate : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Negate();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            context.Machine.Store(context.Next(), (-a));
        }
    }
}
