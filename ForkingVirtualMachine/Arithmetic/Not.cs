namespace ForkingVirtualMachine.Arithmetic
{
    public class Not : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Not();

        public void Execute(Context context)
        {
            var a = context.PopBool();

            context.Push(a
              ? Constants.False
              : Constants.True);
        }
    }
}
