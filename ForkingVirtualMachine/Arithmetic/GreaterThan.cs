namespace ForkingVirtualMachine.Arithmetic
{
    public class GreaterThan : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new GreaterThan();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a > b
                ? Constants.True
                : Constants.False);
        }
    }
}
