namespace ForkingVirtualMachine.State
{
    public class Define : IVirtualMachine
    {
        private VirtualMachine machine;

        public Define(VirtualMachine machine)
        {
            this.machine = machine;
        }

        public void Execute(Execution execution)
        {
            var word = execution.Context.Pop()[0];
            var data = execution.Context.Pop();

            if (word == 0)
            {
                return; // hrm.
            }

            if (data.Length == 0)
            {
                machine.Operations.Remove(word);
            }
            else
            {
                machine.Set(word, new Executable(machine, null, data));
            }
        }
    }
}
