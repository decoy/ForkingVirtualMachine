namespace ForkingVirtualMachine
{
    using System;

    public interface IContext
    {
        public void Push(ReadOnlyMemory<byte> data);
        public void Push(IVirtualMachine machine);
        public ReadOnlyMemory<byte> Pop();

        public void Call(IScope from, byte[] scopeId, ReadOnlyMemory<byte> data);
        public void Define(byte[] word, ReadOnlyMemory<byte> data);

        public IScope Caller { get; }
    }
}
