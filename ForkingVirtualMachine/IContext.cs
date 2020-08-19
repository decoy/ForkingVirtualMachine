namespace ForkingVirtualMachine
{
    using System;

    public interface IContext
    {
        public void Push(ReadOnlyMemory<byte> data);
        public void Push(IExecution machine);
        public ReadOnlyMemory<byte> Pop();
        public bool Pop(out IExecution execution);

        public void Tick();
    }
}
