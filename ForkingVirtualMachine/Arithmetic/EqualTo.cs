namespace ForkingVirtualMachine.Arithmetic
{
    public class EqualTo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new EqualTo();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a == b
                ? Constants.True
                : Constants.False);
        }
    }
}
