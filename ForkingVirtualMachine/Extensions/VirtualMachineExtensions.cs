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
            var context = new Context();
            foreach (var word in machine.Machines.Keys.ToArray())
            {
                context.Functions.Add(word, new Execution(null, new[] { word }));
            }
            return context;
        }

        public static Context Fork(this Context parent, params byte[] words)
        {
            var context = new Context();
            foreach (var word in words)
            {
                context.Functions.Add(word, parent.Functions[word]);
            }
            return context;
        }
    }
}
