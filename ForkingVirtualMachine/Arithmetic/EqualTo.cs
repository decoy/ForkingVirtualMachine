namespace ForkingVirtualMachine.Arithmetic
{
    public class EqualTo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new EqualTo();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            var b = context.Machine.LoadInt(context.Next());

            context.Machine.Store(context.Next(), a == b ? And.True : And.False);
        }
    }
}
