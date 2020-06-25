namespace ForkingVirtualMachine.Extensions
{
    using System.Linq;

    public static class VirtualMachineExtensions
    {
        public static VirtualMachine Add(this VirtualMachine vm, byte word, IVirtualMachine machine)
        {
            vm.Machines.Add(word, machine);
            return vm;
        }

        public static VirtualMachine2 Add(this VirtualMachine2 vm, byte word, IVirtualMachine machine)
        {
            vm.Machines.Add(word, machine);
            return vm;
        }

        public static Context GetExecutionScope(this Context context)
        {
            Context scope = context;
            for (var i = context.Execution.Scope; i > 0; i++)
            {
                // TODO: null refs mean this is corrupted?
                if (context == null)
                {
                    throw new UnknownOperationException(0);
                }
                scope = context.Parent;
            }
            return scope;
        }

        public static Context Fork(this VirtualMachine machine)
        {
            return Fork(null, machine.Machines.Keys.ToArray());
        }

        public static Context Fork(this VirtualMachine2 machine)
        {
            return Fork(null, machine.Machines.Keys.ToArray());
        }

        public static Context Fork(this Context parent, params byte[] words)
        {
            var context = new Context(parent);
            foreach (var word in words)
            {
                context.Functions.Add(word, new Execution(new byte[] { word }, 1));
            }
            return context;
        }

        public static void Run(this IVirtualMachine machine, Context context)
        {
            while (context.Executions.Count > 0)
            {
                while (!context.Execution.IsComplete)
                {
                    machine.Execute(context);
                }
                context.Executions.Pop();
            }
        }
    }
}
