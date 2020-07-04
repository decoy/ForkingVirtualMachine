namespace ForkingVirtualMachine
{
    using System.Numerics;

    public static class ContextExtensions
    {
        public static bool PopBool(this Context context)
        {
            return PopInt(context) != 0;
        }

        public static BigInteger PopInt(this Context context)
        {
            var data = context.Pop();
            if (data.Length > Constants.MAX_INT_BYTES)
            {
                throw new BoundaryException();
            }
            return new BigInteger(data);
        }

        public static void Push(this Context context, BigInteger integer)
        {
            context.Push(integer.ToByteArray());
        }
    }
}
