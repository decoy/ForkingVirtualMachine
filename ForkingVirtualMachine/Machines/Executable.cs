using System;

namespace ForkingVirtualMachine.Machines
{
    public class Executable : IVirtualMachine
    {
        public readonly ReadOnlyMemory<byte> Id;

        public readonly ReadOnlyMemory<byte> Data;

        private readonly IDescribe scope;

        public Executable(IDescribe scope, ReadOnlyMemory<byte> data)
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
