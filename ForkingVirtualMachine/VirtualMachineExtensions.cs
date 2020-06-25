namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Machines;
    using System.Collections.Generic;
    using System.Linq;

    public static class VirtualMachineExtensions
    {
        public static IVirtualMachine Fork(this IVirtualMachine machine, byte[] words)
        {
            var context = new Context();
            foreach (var word in words)
            {
                context.Functions.Add(word, new Execution(new byte[] { 0, word }));
            }
            return new ContextMachine(machine, context);
        }

        public static void Run(this IVirtualMachine machine, IEnumerable<byte> program)
        {
            var exe = new Execution(program.ToArray());
            machine.State.Executions.Push(exe);
        }

        //public static void Run(this ForkedVM context, IEnumerable<byte> stream)
        //{
        //    context.Run(stream.GetEnumerator());
        //}
    }
}
