namespace ForkingVirtualMachine
{
    using System;

    public interface ICallScheduler
    {
        public void Schedule(IScope caller, byte[] scopeId, ReadOnlyMemory<byte> data);
    }
}
