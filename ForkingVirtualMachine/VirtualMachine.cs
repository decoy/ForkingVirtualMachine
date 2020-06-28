namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Arithmetic;
    using System;
    using System.Collections.Generic;

    public class VirtualMachine : IVirtualMachine
    {
        public const int MAX_REGISTER_SIZE = 1024 * 1024;

        public const int MAX_DEPTH = 1024;

        public static readonly ReadOnlyMemory<byte> Empty = new byte[0];

        private int depth = 0;

        private readonly Dictionary<byte, Executable> Registers = new Dictionary<byte, Executable>();

        public static void Run(IVirtualMachine machine, Context exe)
        {
            while (!exe.IsComplete)
            {
                machine.Execute(exe);
            }
        }

        public void Execute(Context context)
        {
            if (context.Machine.depth >= MAX_DEPTH)
            {
                throw new Exception("boom");
            }

            var op = context.Next();
            if (!Registers.ContainsKey(op))
            {
                throw new Exception("unknown op: " + op);
            }

            var next = Registers[op];
            if (next.Machine == this)
            {
                var exe = new Context(this, next.Data.ToArray());
                Run(next.Machine, exe);
            }
            else
            {
                next.Machine.Execute(context);
            }
        }

        public void Set(byte word, Executable exe)
        {
            if (Registers.ContainsKey(word))
            {
                Registers[word] = exe;
            }
            else
            {
                Registers.Add(word, exe);
            }
        }

        public ReadOnlyMemory<byte> Load(byte word)
        {
            if (Registers.ContainsKey(word))
            {
                return Registers[word].Data;
            }
            return Empty;
        }

        public void Store(byte word, byte[] data)
        {
            if (word == 0)
            {
                return; // throw it away
            }

            if (data == null || data.Length == 0)
            {
                Registers.Remove(word);
                return;
            }

            if (data.Length > MAX_REGISTER_SIZE)
            {
                throw new BoundaryException();
            }

            var exe = new Executable(this, null, data);
            Set(word, exe);
        }
    }
}
