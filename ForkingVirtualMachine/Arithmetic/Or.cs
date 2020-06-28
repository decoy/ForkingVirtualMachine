namespace ForkingVirtualMachine.Arithmetic
{
    public class Or : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Or();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            var b = context.Machine.LoadInt(context.Next());
            context.Machine.Store(context.Next(), ((a != 0) || (b != 0)) ? And.True : And.False);
        }
    }
}
