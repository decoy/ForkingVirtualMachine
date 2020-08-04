namespace ForkingVirtualMachine
{
    using System.Numerics;

    public static class ContextExtensions
    {
        public static bool PopBool(this IContext context)
        {
            return PopInt(context) != 0;
        }

        public static BigInteger PopInt(this IContext context)
        {
            var data = context.Pop();
            if (data.Length > Constants.MAX_INT_BYTES)
            {
                throw new BoundaryException();
            }
            return new BigInteger(data.Span);
        }

        public static void Push(this IContext context, bool value)
        {
            context.Push(value ? Constants.True : Constants.False);
        }

        public static void Push(this IContext context, BigInteger integer)
        {
            context.Push(integer.ToByteArray());
        }
    }
}
