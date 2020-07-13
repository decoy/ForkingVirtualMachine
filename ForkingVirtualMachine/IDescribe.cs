namespace ForkingVirtualMachine
{
    using System;

    public interface IDescribe
    {
        public IVirtualMachine Describe(ReadOnlyMemory<byte> word);
    }
}
