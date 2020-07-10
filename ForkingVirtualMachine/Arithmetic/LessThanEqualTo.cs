namespace ForkingVirtualMachine.Arithmetic
{
    public class LessThanEqualTo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new LessThanEqualTo();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a <= b
                ? Constants.True
                : Constants.False);
        }
    }
}
