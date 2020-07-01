﻿namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    public class Manager
    {
        private HashAlgorithm hasher = SHA256.Create();

        public readonly Dictionary<string, Executable> Machines;

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

        public void Save(VirtualMachine machine)
        {



            //var me = machine.Operations.Select(op =>
            //{
            //new byte[] {
            //       Constants.Push32,
            //       op.Value.Data.Length,
            //       Constants.Push8,
            //       op.Key
            //});

            foreach (var op in machine.Operations)
            {
                var id = Save(op.Value);

                // if scope == 0 it belongs to this one.
                var scope = op.Value.Scope;


            }
        }

        public ReadOnlySpan<byte> Save(Executable executable)
        {
            if (executable.Id.Length == 0)
            {
                var id = new Span<byte>(new byte[hasher.HashSize]);

                // does this do i/o? for the try?
                if (hasher.TryComputeHash(executable.Data, id, out var _))
                {
                    return id;
                }
            }

            return null;
        }
    }
}
