namespace ForkingVirtualMachine.Arithmetic
{
    public class Or : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Or();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadBool(context.Next());
            var b = context.Machine.LoadBool(context.Next());
            context.Machine.Store(context.Next(), (a || b) ? And.True : And.False);
        }
    }
}
