namespace ForkingVirtualMachine.Arithmetic
{
    using System.Numerics;

    public static class MachineExtensions
    {
        public static bool LoadBool(this VirtualMachine machine, byte word)
        {
            return new BigInteger(machine.Load(word).Span) != 0;
        }

        public static BigInteger LoadInt(this VirtualMachine machine, byte word)
        {
            return new BigInteger(machine.Load(word).Span);
        }

        public static void Store(this VirtualMachine machine, byte word, BigInteger integer)
        {
            machine.Store(word, integer.ToByteArray());
        }
    }
}
