namespace ForkingVirtualMachine.State
{
    public class Define : IVirtualMachine
    {
        private VirtualMachine local;

        public Define(VirtualMachine local)
        {
            this.local = local;
        }

        public void Execute(Execution execution)
        {
            var word = execution.Context.Pop();
            var data = execution.Context.Pop();
            local.Store(word[0], data);
        }
    }
}
