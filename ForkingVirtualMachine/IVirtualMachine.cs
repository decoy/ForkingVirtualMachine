namespace ForkingVirtualMachine
{
    public interface IVirtualMachine
    {
        //public byte[] Id { get; }

        public void Execute(IScope scope, IContext context);
    }
}
