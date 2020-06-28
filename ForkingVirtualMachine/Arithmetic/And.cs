namespace ForkingVirtualMachine.Arithmetic
{
    using System.Numerics;

    public class And : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new And();

        public static readonly byte[] True = new byte[1] { 1 };

        public static readonly byte[] False = new byte[0];

        public void Execute(Context context)
        {
            var a = context.Machine.LoadInt(context.Next());
            var b = context.Machine.LoadInt(context.Next());
            context.Machine.Store(context.Next(), ((a != 0) && (b != 0)) ? And.True : And.False);
        }
    }
}
