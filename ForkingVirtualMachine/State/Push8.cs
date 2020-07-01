namespace ForkingVirtualMachine.State
{
    public class Push8 : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Push8();

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
