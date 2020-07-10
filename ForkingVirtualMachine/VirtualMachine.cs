namespace ForkingVirtualMachine
{
    using System;
    using System.Buffers.Binary;

    public class VirtualMachine : IVirtualMachine
    {
        private readonly IVirtualMachine loader;

        private readonly ReadOnlyMemory<byte> data;
        private int len;
        private int i;

        public VirtualMachine(IVirtualMachine loader, ReadOnlyMemory<byte> data)
        {
            this.loader = loader;
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
                    context.Ticks++;
                    if (context.Ticks > Constants.MAX_TICKS)
                    {
                        throw new BoundaryException();
                    }

                    context.Push(this); // save our spot

                    loader.Execute(context);
                    return;
                }

                if (len == byte.MaxValue)
                {
                    // TODO: less arbitrary here?
                    // (maybe 255 = read next two bytes as len?, 255,255 = 4?)
                    len = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(i));
                    i += 4;
                }

                context.Push(data.Slice(i, len));
                i += len;
            }
        }
    }
}
