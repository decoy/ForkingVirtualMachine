namespace ForkingVirtualMachine.Machines
{
    using System;
    using System.Buffers.Binary;

    public class Execution : IExecution
    {
        private readonly IScope scope;

        private readonly ReadOnlyMemory<byte> data;
        private int len;
        private int i;

        public Execution(IScope scope, ReadOnlyMemory<byte> data)
        {
            this.scope = scope;
            this.data = data;
        }

        public void Execute(IContext context)
        {
            if (data.Length == i)
            {
                return;
            }

            var span = data.Span;

            while (data.Length != i)
            {
                len = span[i];
                i++;

                if (len == Constants.EXECUTE)
                {
                    context.Tick();
                    context.Push(this); // save our spot
                    context.Push(scope);
                    return;
                }

                if (len == byte.MaxValue)
                {
                    // TODO: less arbitrary here?
                    len = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(i));
                    i += 4;
                }

                context.Push(data.Slice(i, len));
                i += len;
            }
        }
    }
}
