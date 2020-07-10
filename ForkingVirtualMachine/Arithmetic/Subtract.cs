namespace ForkingVirtualMachine.Arithmetic
{
    public class Subtract : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Subtract();

        public void Execute(Context context)
        {
            var a = context.PopInt();
            var b = context.PopInt();
            context.Push(a - b);
        }
    }
}
