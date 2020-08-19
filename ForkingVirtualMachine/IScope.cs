namespace ForkingVirtualMachine
{
    public interface IScope : IExecution
    {
        public byte[] Id { get; }

        public byte[] FromId { get; }

        public byte[] ToId { get; }

        public IScope Caller { get; }

        public IVirtualMachine Machine { get; }
    }
}
