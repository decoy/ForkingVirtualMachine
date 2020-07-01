namespace ForkingVirtualMachine
{
    public class Executable
    {
        public readonly byte[] Id;

        public readonly byte[] Scope;

        public readonly byte[] Data;

        public readonly IVirtualMachine Machine;

        public Executable(IVirtualMachine machine, byte[] id, byte[] data)
        {
            Id = id;
            Data = data;
            Machine = machine;
        }
    }
}
