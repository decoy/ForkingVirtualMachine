namespace ForkingVirtualMachine.Machines
{
    public class LessThan : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new LessThan();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a < b
                ? Constants.True
                : Constants.False);
        }
    }
}
