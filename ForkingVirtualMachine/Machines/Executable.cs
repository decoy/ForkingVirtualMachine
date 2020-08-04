namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Executable : IVirtualMachine
    {
        private readonly ReadOnlyMemory<byte> data;

        private readonly IScope scope;

        public Executable(IScope scope, ReadOnlyMemory<byte> data)
        {
            this.data = data;
            this.scope = scope;
        }

        public void Execute(IContext context)
        {
            context.Push(new Execution(scope, data));
        }
    }
}
