namespace ForkingVirtualMachine.State
{
    using System;

    public class Push32 : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Push32();

        public void Execute(Execution execution)
        {
            var len = BitConverter.ToInt32(execution.Next(4));

            if (len <= 0)
            {
                return;
            }

            var data = execution.Next(len);
            execution.Context.Push(data.ToArray());
        }
    }
}
