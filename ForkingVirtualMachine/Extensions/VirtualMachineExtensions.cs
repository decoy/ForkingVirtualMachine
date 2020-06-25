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

        public static Context Fork(this VirtualMachine machine)
        {
            return Fork(null, machine.Machines.Keys.ToArray());
        }

        public static Context Fork(this Context parent, params byte[] words)
        {
            var context = new Context(parent);
            foreach (var word in words)
            {
                context.Functions.Add(word, new Execution(new byte[] { word }));
            }
            return context;
        }

        public static void Run(this IVirtualMachine machine, Context context)
        {
            while (context.Executions.Count > 0)
            {
                machine.Execute(context);
            }
        }
    }
}
