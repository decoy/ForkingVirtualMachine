namespace ForkingVirtualMachine
{
    public interface IScope : IVirtualMachine
    {
        public byte[] Id { get; }
    }
}
