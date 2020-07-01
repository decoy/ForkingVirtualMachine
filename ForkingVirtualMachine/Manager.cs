using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ForkingVirtualMachine
{
    public class Manager
    {
        private HashAlgorithm hasher = SHA256.Create();

        public Dictionary<string, Executable> Machines { get; }

        public Manager()
        {
            Machines = new Dictionary<string, Executable>();
        }

        public Manager(Dictionary<string, Executable> machines)
        {
            Machines = machines;
        }

        public Executable Add(IVirtualMachine machine, byte word)
        {
            return Add(machine, new[] { word });
        }

        public Executable Add(IVirtualMachine machine, byte[] data)
        {
            var hash = hasher.ComputeHash(data);
            var key = Convert.ToBase64String(hash);
            var exe = new Executable(machine, hash, data);

            if (Machines.ContainsKey(key))
            {
                throw new Exception($"Operation {key} is already defined with {Machines[key].GetType().FullName}");
            }

            Machines.Add(key, exe);
            return exe;
        }

        public Executable Load(byte[] id)
        {
            var key = Convert.ToBase64String(id);
            return Machines[key];
        }

        public void Save(Execution execution)
        {
            //foreach (var exe in context.Machine.reg)
            //{

            //}
        }

        public void Save(Executable executable)
        {
            if (executable.Id.Length == 0)
            {
                var id = new Span<byte>(new byte[hasher.HashSize]);

                // does this do i/o? for the try?
                if (hasher.TryComputeHash(executable.Data, id, out var _))
                {
                }
            }
        }
    }
}
