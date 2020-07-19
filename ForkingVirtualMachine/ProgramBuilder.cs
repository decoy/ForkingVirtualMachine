namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Numerics;

    public static class ProgramBuilder
    {
        public static byte EXECUTE = Constants.EXECUTE;
        public static byte DEFINE = Constants.PUSH;

        public static byte[] Create(Action<Stream> create)
        {
            using (var stream = new MemoryStream())
            {
                create(stream);
                return stream.ToArray();
            }
        }

        public static Stream Execute(this Stream stream, byte word)
        {
            Push(stream, word);
            stream.WriteByte(EXECUTE);
            return stream;
        }

        public static Stream Execute(this Stream stream, BigInteger word)
        {
            Push(stream, word);
            stream.WriteByte(EXECUTE);
            return stream;
        }

        public static Stream Execute(this Stream stream, ReadOnlySpan<byte> word)
        {
            Push(stream, word);
            stream.WriteByte(EXECUTE);
            return stream;
        }

        public static Stream Push(this Stream stream, ReadOnlySpan<byte> data)
        {
            if (data.Length >= 255)
            {
                stream.WriteByte(255);
                stream.Write(BitConverter.GetBytes(data.Length));
            }
            else
            {
                stream.WriteByte((byte)data.Length);
            }

            stream.Write(data);

            return stream;
        }

        public static Stream Push(this Stream stream, byte data)
        {
            stream.WriteByte(1);
            stream.WriteByte(data);
            return stream;
        }

        public static Stream Push(this Stream stream, BigInteger data)
        {
            Push(stream, data.ToByteArray());
            return stream;
        }


        public static Stream Push(this Stream stream, Action<Stream> build)
        {
            Push(stream, Create(build));
            return stream;
        }

        public static Stream Define(this Stream stream, byte word, Action<Stream> build)
        {
            Push(stream, build);
            Push(stream, word);
            Execute(stream, DEFINE);
            return stream;
        }
    }
}
