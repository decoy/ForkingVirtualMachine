namespace ForkingVirtualMachine.Arithmetic
{
    public class Not : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Not();

        public void Execute(Context context)
        {
            var a = context.Machine.LoadBool(context.Next());

            context.Machine.Store(context.Next(), a ? And.False : And.True);
        }
    }
}
