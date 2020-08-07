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
            Execute(this, context);
        }

        public void Execute(IScope scope, IContext context)
        {
            var word = context.Pop().ToArray();
            machines[word].Execute(scope, context);
        }

        public void Set(byte[] word, IVirtualMachine machine)
        {
            machines.Set(word, machine);
        }

        public bool Has(byte[] word)
        {
            return machines.ContainsKey(word);
        }

        public void Remove(byte[] word)
        {
            machines.Remove(word);
        }


    }
}
