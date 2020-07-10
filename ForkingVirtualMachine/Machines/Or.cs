namespace ForkingVirtualMachine.Machines
{
    public class Or : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Or();

        public void Execute(Context context)
        {
            var a = context.PopBool();
            var b = context.PopBool();
            context.Push((a || b)
                ? Constants.True
                : Constants.False);
        }
    }
}
