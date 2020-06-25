namespace ForkingVirtualMachine
{
    using System;

    public class Execution
    {
        private ReadOnlyMemory<byte> data;

        public int Scope { get; }

        public int Index { get; private set; }

        public int Length => data.Length;

        public byte Current => data.Span[Index];

        public bool IsComplete => Index >= data.Length;

        public Execution(byte[] data, int scope)
        {
            this.Scope = scope;
            this.data = data;
        }

        public byte Next()
        {
            var res = Current;
            Index++;
            return res;
        }

        public ReadOnlySpan<byte> Next(int len)
        {
            var res = data.Span.Slice(Index, len);
            Index += len;
            return res;
        }

        public ReadOnlySpan<byte> ToBytes()
        {
            return data.Span.Slice(Index);
        }

        public Execution ToScope(int scope)
        {
            return new Execution(ToBytes().ToArray(), Scope + scope);
        }
    }
}
