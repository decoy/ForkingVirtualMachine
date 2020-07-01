namespace ForkingVirtualMachine
{
    using System.Numerics;

    public static class ContextExtensions
    {
        public static bool PopBool(this Context context)
        {
            return new BigInteger(context.Pop()) != 0;
        }

        public static BigInteger PopInt(this Context context)
        {
            return new BigInteger(context.Pop());
        }

        public static void Push(this Context context, BigInteger integer)
        {
            context.Push(integer.ToByteArray());
        }
    }
}
