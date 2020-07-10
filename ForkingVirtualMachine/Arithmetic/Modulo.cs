namespace ForkingVirtualMachine.Arithmetic
{
    public class Modulo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Modulo();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a % b);
        }
    }
}
