using System;

namespace ForkingVirtualMachine.Machines
{
    public class Executable : IVirtualMachine
    {
        public readonly ReadOnlyMemory<byte> Data;

        private readonly IVirtualMachine scope;

        public Executable(IVirtualMachine scope, ReadOnlyMemory<byte> data)
        {
            Data = data;
            this.scope = scope;
        }

        public void Execute(Context context)
        {
            context.Push(new VirtualMachine(scope, Data));
        }
    }
}
