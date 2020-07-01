namespace ForkingVirtualMachine
{
    using System;

    public class Execution
    {
        public readonly Context Context;

        public bool IsComplete => IsStopped || i == data.Length;

        public bool IsStopped { get; private set; }

        private readonly byte[] data;
        private int i;

        public Execution(Context context, byte[] data)
        {
            Context = context;
            this.data = data;
        }

        public void Stop()
        {
            IsStopped = true;
        }

        public byte Next()
        {
            var res = data[i];
            i++;
            return res;
        }

        public ReadOnlySpan<byte> Next(int len)
        {
            var res = new ReadOnlySpan<byte>(data, i, len);
            i += len;
            return res;
        }
    }
}
