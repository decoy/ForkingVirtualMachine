namespace ForkingVirtualMachine.State
{
    public class Push : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Push();

        public void Execute(Execution execution)
        {
            var len = execution.Next();

            if (len == 0)
            {
                return;
            }

            var data = execution.Next(len);
            execution.Context.Push(data.ToArray());
        }
    }
}
