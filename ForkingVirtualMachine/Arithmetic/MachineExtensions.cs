namespace ForkingVirtualMachine.Arithmetic
{
    using System.Numerics;

    public static class MachineExtensions
    {
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
