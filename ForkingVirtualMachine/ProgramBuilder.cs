namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class ProgramBuilder
    {
        private readonly List<byte> program = new List<byte>();

        public static ProgramBuilder Create()
        {
            return new ProgramBuilder();
        }

        public byte[] ToBytes()
        {
            return program.ToArray();
        }

        public ProgramBuilder Push(params BigInteger[] numbers)
        {
            Push(numbers.Select(n => n.ToByteArray()));
            return this;
        }

        public ProgramBuilder Push(IEnumerable<byte[]> buffers)
        {
            foreach (var data in buffers)
            {
                Push(data);
            }
            return this;
        }

        public ProgramBuilder Push(byte[] data)
        {
            if (data.Length >= 255)
            {
                program.Add(255);
                program.AddRange(BitConverter.GetBytes(data.Length));
            }
            else
            {
                program.Add((byte)data.Length);
            }

            program.AddRange(data);

            return this;
        }

        public ProgramBuilder Add(params byte[] data)
        {
            program.AddRange(data);
            return this;
        }

        public ProgramBuilder Execute(params byte[] words)
        {
            foreach (var word in words)
            {
                Push(word);
                program.Add(Constants.EXECUTE);
            }

            return this;
        }

        public ProgramBuilder Define(byte word, Action<ProgramBuilder> build)
        {
            var next = new ProgramBuilder();
            build(next);

            Push(next.ToBytes());
            Push(word);
            Execute(Constants.PUSH); // store

            return this;
        }
    }
}
