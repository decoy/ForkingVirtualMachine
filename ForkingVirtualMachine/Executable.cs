namespace ForkingVirtualMachine
{
    using System;

    public class Executable
    {
        public readonly ReadOnlyMemory<byte> Id;

        public readonly ReadOnlyMemory<byte> Scope;

        public readonly ReadOnlyMemory<byte> Data;

        public readonly IVirtualMachine Machine;

        public Executable(IVirtualMachine machine, ReadOnlyMemory<byte> id, ReadOnlyMemory<byte> data)
        {
            Id = id;
            Data = data;
            Machine = machine;
        }
    }
}
