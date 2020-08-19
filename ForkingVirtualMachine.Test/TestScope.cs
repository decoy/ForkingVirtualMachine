namespace ForkingVirtualMachine.Test
{
    public class TestScope : IScope
    {
        public byte[] Id { get; }

        public byte[] FromId { get; }

        public byte[] ToId { get; }

        public IScope Caller { get; }

        public IVirtualMachine Machine { get; }

        public TestScope(byte[] id, byte[] fromId, byte[] toId, IScope caller, IVirtualMachine machine)
        {
            Id = id;
            FromId = fromId;
            ToId = toId;
            Caller = caller;
            Machine = machine;
        }

        public TestScope(IScope caller, IVirtualMachine machine)
        {
            Caller = caller;
            Machine = machine;
        }

        public void Execute(IContext context)
        {
            Machine.Execute(this, context);
        }
    }
}
