namespace ForkingVirtualMachine.Extensions
{
    using System.Collections.Generic;

    public static class ProgramBuilderExtensions
    {
        public static List<byte> Add(this List<byte> program, params byte[] data)
        {
            program.AddRange(data);
            return program;
        }

        public static List<byte> AddData(this List<byte> program, params byte[] data)
        {
            program.Add((byte)data.Length);
            program.AddRange(data);
            return program;
        }
    }
}
