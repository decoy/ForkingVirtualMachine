namespace ForkingVirtualMachine
{
    using System;

    public class Execution
    {
        public readonly IVirtualMachine Machine;

        public bool IsComplete => i == data.Length;

        private readonly byte[] data;
        private int i;

        public Execution(Executable exe)
        {
            Machine = exe.Machine;
            data = exe.Data.ToArray();
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
