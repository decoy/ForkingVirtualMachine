namespace ForkingVirtualMachine
{
    public interface IScope : IVirtualMachine, IExecution
    {
        public byte[] Id { get; }
        public void Set(byte[] word, IVirtualMachine machine);
        public bool Has(byte[] word);
        public void Remove(byte[] word);
    }
}
