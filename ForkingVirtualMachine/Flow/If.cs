namespace ForkingVirtualMachine.Flow
{
    using System.Numerics;

    public class If : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new If();

        public void Execute(Context context)
        {
            var x = new BigInteger(context
                .Machine
                .Load(context.Next())
                .Span);

            if (x != 0)
            {
                context.Next();
            }
        }
    }
}
