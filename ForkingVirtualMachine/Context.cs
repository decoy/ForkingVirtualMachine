namespace ForkingVirtualMachine
{
    using System;

    public class Context
    {
        public readonly VirtualMachine Machine;

        public bool IsComplete => i == data.Length;

        private readonly byte[] data;
        private int i;

        public Context(VirtualMachine machine, byte[] data)
        {
            Machine = machine;
            this.data = data;
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
