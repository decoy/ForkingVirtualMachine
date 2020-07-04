namespace ForkingVirtualMachine.Store
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
            if (exe.Machine == null)
            {
                exe = new Executable(machine, exe.Data, exe.Data);
            }
            machine.Set(word, exe);
        }
    }
}
