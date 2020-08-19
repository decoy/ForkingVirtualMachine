namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Machines;
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

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

        public static async Task RunAsync(this IContext context)
        {
            while (context.Pop(out var execution))
            {
                if (execution is IAsyncExecution aex)
                {
                    await aex.ExecuteAsync(context);
                }
                else
                {
                    execution.Execute(context);
                }
            }
        }

        public static void Run(this IContext context)
        {
            while (context.Pop(out var execution))
            {
                execution.Execute(context);
            }
        }

        public static void Run(this IContext context, IScope scope, ReadOnlyMemory<byte> data)
        {
            new Execution(scope, data).Execute(context);
            context.Run();
        }
    }
}
