namespace ForkingVirtualMachine.Extensions
{
    using ForkingVirtualMachine.Machines;

    public static class VirtualMachineExtensions
    {
        public static VirtualMachine Add(this VirtualMachine vm, byte word, IVirtualMachine machine)
        {
            vm.Machines.Add(word, machine);
            return vm;
        }

        public static ContextMachine Fork(this IVirtualMachine machine, params byte[] words)
        {
            var context = new Context();
            foreach (var word in words)
            {
                context.Functions.Add(word, new Execution(new byte[] { word }));
            }
            return new ContextMachine(machine, context);
        }

        public static void Run(this ContextMachine machine, Execution exe)
        {
            machine.State.Executions.Push(exe);
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
