namespace ForkingVirtualMachine.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ProgramBuilderExtensions
    {
        public static List<byte> AddProgram(this List<byte> program, params byte[] data)
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
