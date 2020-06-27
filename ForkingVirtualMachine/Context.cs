namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;

    public class Context
    {
        public readonly Dictionary<byte, Executable> Functions = new Dictionary<byte, Executable>();
        public readonly Stack<long> Stack = new Stack<long>();

        public bool IsComplete => i == data.Length;

        private readonly byte[] data;
        private int i;

        public Context(byte[] data)
        {
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
