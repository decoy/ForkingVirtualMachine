namespace ForkingVirtualMachine
{
    using System;

    public class Scope : IDescribe
    {
        public readonly ReadOnlyMemory<byte> Id;

        private readonly Store<IVirtualMachine> machines;

        public Scope(ReadOnlyMemory<byte> id)
        {
            Id = id;
            machines = new Store<IVirtualMachine>();
        }

        public Scope(ReadOnlyMemory<byte> id, Store<IVirtualMachine> machines)
        {
            Id = id;
            this.machines = machines;
        }

        public void Set(byte key, IVirtualMachine machine)
        {
            Set(new byte[] { key }, machine);
        }

        public void Set(byte[] key, IVirtualMachine machine)
        {
            if (machines.ContainsKey(key))
            {
                machines[key] = machine;
            }
            else
            {
                machines.Add(key, machine);
            }
        }

        public IVirtualMachine Describe(ReadOnlyMemory<byte> word)
        {
            var key = word.ToArray();
            if (machines.ContainsKey(key))
            {
                return machines[key];
            }

            return null;
        }

        public Scope Fork(ReadOnlyMemory<byte> id)
        {
            return new Scope(id, new Store<IVirtualMachine>(machines));
        }
    }
}
