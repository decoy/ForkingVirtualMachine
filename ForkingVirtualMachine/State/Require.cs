namespace ForkingVirtualMachine.State
{
    public class Require : IVirtualMachine
    {
        private Manager manager;
        private VirtualMachine machine;

        public Require(VirtualMachine machine, Manager manager)
        {
            this.machine = machine;
            this.manager = manager;
        }

        public void Execute(Execution execution)
        {
            var word = execution.Context.Pop()[0];
            var id = execution.Context.Pop();

            if (word == 0)
            {
                return;
            }

            var exe = manager.Load(id);
            machine.Set(word, exe);
        }
    }
}
