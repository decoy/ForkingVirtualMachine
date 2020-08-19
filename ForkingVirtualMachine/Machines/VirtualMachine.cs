namespace ForkingVirtualMachine.Machines
{
    using ForkingVirtualMachine.Utility;

    public class VirtualMachine : IVirtualMachine
    {
        public byte[] Id { get; }

        private readonly Store<IVirtualMachine> machines;

        public VirtualMachine(byte[] id, Store<IVirtualMachine> machines)
        {
            Id = id;
            this.machines = machines;
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

        public bool TryGet(byte[] word, out IVirtualMachine machine)
        {
            return machines.TryGetValue(word, out machine);
        }
    }
}
