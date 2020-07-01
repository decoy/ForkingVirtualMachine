namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class ProgramBuilder
    {
        private readonly List<byte> program = new List<byte>();

        private readonly byte define;
        private readonly byte push8;
        private readonly byte push32;

        public ProgramBuilder(byte define, byte push8, byte push32)
        {
            this.define = define;
            this.push8 = push8;
            this.push32 = push32;
        }

        public static ProgramBuilder Create()
        {
            return new ProgramBuilder(Constants.Define, Constants.Push8, Constants.Push32);
        }

        public byte[] ToBytes()
        {
            return program.ToArray();
        }

        public ProgramBuilder Define(byte word, Action<ProgramBuilder> build)
        {
            var next = new ProgramBuilder(define, push8, push32);
            build(next);

            Define(word, next.ToBytes());

            return this;
        }

        public ProgramBuilder Define(byte word, params byte[] data)
        {
            Push(data);
            Push(word);
            program.Add(define);

            return this;
        }

        public ProgramBuilder Add(params byte[] data)
        {
            program.AddRange(data);
            return this;
        }

        public ProgramBuilder Push(BigInteger number)
        {
            var x = number.ToByteArray();
            Push(number.ToByteArray());
            return this;
        }

        public ProgramBuilder Push(params byte[] data)
        {
            if (data.Length > byte.MaxValue)
            {
                program.Add(push32);
                program.AddRange(BitConverter.GetBytes(data.Length));
            }
            else
            {
                program.Add(push8);
                program.Add((byte)data.Length);
            }

            program.AddRange(data);

            return this;
        }
    }
}
