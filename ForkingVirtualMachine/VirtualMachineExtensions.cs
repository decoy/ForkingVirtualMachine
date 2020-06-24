namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public static class VirtualMachineExtensions
    {
        public static Context Fork(this IVirtualMachine machine, byte[] words)
        {
            var context = new Context(machine);
            foreach (var word in words)
            {
                context.Set(word, new byte[] { VirtualMachine.ParentScope });
            }
            return context;
        }

        public static void Run(this Context context, IEnumerable<byte> stream)
        {
            context.Run(stream.GetEnumerator());
        }
    }
}
