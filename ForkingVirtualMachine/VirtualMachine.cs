namespace ForkingVirtualMachine
{
    using System;
    using System.Buffers.Binary;

    public class VirtualMachine : IVirtualMachine
    {
        private readonly IDescribe scope;

        private readonly ReadOnlyMemory<byte> data;
        private int len;
        private int i;

        public VirtualMachine(IDescribe loader, ReadOnlyMemory<byte> data)
        {
            this.scope = loader;
            this.data = data;
        }

        public void Execute(Context context)
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
                    context.Push(scope.Describe(context.Pop()));
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
