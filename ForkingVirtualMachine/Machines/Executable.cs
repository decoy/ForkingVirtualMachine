namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Executable : IVirtualMachine
    {
        private readonly ReadOnlyMemory<byte> data;

        public Executable(ReadOnlyMemory<byte> data)
        {
            this.data = data;
        }

        public void Execute(IScope scope, IContext context)
        {
            context.Push(new Execution(scope, data));
        }
    }
}
