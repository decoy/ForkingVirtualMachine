namespace ForkingVirtualMachine.Machines
{
    using ForkingVirtualMachine.Utility;

    public class Scope : IScope
    {
        public byte[] Id { get; }

        private readonly Store<IVirtualMachine> machines;

        public Scope(byte[] id, Store<IVirtualMachine> machines)
        {
            Id = id;
            this.machines = machines;
        }

        public void Execute(IContext context)
        {
            var word = context.Pop().ToArray();
            if (word.IsZero())
            {
                var scopeid = context.Pop().ToArray();
                var data = context.Pop();
                context.Call(this, scopeid, data);
                return;
            }

            machines[word].Execute(context);
        }
    }
}
