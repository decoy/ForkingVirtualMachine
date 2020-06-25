namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Machines;
    using System.Collections.Generic;
    using System.Linq;

    public static class VirtualMachineExtensions
    {
        public static ContextMachine Fork(this IVirtualMachine machine, byte[] words)
        {
            var context = new Context();
            foreach (var word in words)
            {
                context.Functions.Add(word, new Execution(new byte[] { word }));
            }
            return new ContextMachine(machine, context);
        }

        public static void Run(this ContextMachine machine, IEnumerable<byte> program)
        {
            machine.State.Executions.Push(new Execution(program.ToArray()));
            while (machine.State.Executions.Count > 0)
            {
                while (!machine.State.Execution.IsComplete)
                {
                    machine.Execute(machine.State);
                }
                machine.State.Executions.Pop();
            }
        }
    }
}
