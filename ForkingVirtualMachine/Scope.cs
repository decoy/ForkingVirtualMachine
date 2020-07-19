namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Utility;
    using System;
    using System.Collections.Generic;

    public class Scope : IDescribe
    {
        public readonly ReadOnlyMemory<byte> Id;

        private readonly Dictionary<byte[], IVirtualMachine> machines;

        public Scope(ReadOnlyMemory<byte> id)
        {
            Id = id;
            machines = machines.Fork();
        }

        public Scope(ReadOnlyMemory<byte> id, Dictionary<byte[], IVirtualMachine> machines)
        {
            Id = id;
            this.machines = machines.Fork();
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
            return new Scope(id, machines.Fork());
        }
    }
}
