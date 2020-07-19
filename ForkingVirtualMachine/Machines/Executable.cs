namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Executable : IVirtualMachine
    {
        private readonly ReadOnlyMemory<byte> data;

        private readonly IDescribe scope;

        public Executable(IDescribe scope, ReadOnlyMemory<byte> data)
        {
            this.data = data;
            this.scope = scope;
        }

        public void Execute(Context context)
        {
            context.Push(new Execution(scope, data));
        }
    }
}
