namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class VirtualMachine : IVirtualMachine
    {
        public readonly Dictionary<byte, Executable> Operations = new Dictionary<byte, Executable>();

        public static void Run(IVirtualMachine machine, Execution execution)
        {
            while (!execution.IsComplete)
            {
                if (execution.Context.Ticks >= Constants.MAX_TICKS || execution.Context.Depth >= Constants.MAX_DEPTH)
                {
                    throw new BoundaryException();
                }

                execution.Context.Ticks++;

                machine.Execute(execution);
            }
        }

        public void Execute(Execution execution)
        {
            var op = execution.Next();
            if (!Operations.ContainsKey(op))
            {
                return; // NoOp
            }

            var next = Operations[op];

            if (next.Data != null && next.Data.Length > 0)
            {
                execution.Context.Depth++;
                var exe = new Execution(execution.Context, next.Data);
                Run(next.Machine, exe);
                execution.Context.Depth--;
            }
            else
            {
                next.Machine.Execute(execution);
            }
        }

        public void Set(byte word, Executable exe)
        {
            if (Operations.ContainsKey(word))
            {
                Operations[word] = exe;
            }
            else
            {
                Operations.Add(word, exe);
            }
        }
    }
}
