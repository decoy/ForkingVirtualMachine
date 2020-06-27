using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
            var data = new[] { word };
            var hash = hasher.ComputeHash(data);
            var key = Convert.ToBase64String(hash);
            var exe = new Executable(machine, hash, new byte[] { word });
            Machines.Add(key, exe);
            return exe;
        }

        public Executable Load(byte[] id)
        {
            var key = Convert.ToBase64String(id);
            return Machines[key];
        }

        public void Save(Context context)
        {
            foreach (var exe in context.Functions)
            {
                
            }
        }

        public void Save(Executable executable)
        {
            if (executable.Id.IsEmpty)
            {
                var id = new Span<byte>(new byte[hasher.HashSize]);

                // does this do i/o? for the try?
                if (hasher.TryComputeHash(executable.Data.Span, id, out var _))
                {
                }
            }
        }
    }
}
