namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class VirtualMachine : IVirtualMachine
    {
        private readonly Dictionary<byte, Executable> Operations = new Dictionary<byte, Executable>();

        public static void Run(IVirtualMachine machine, Execution exe)
        {
            while (!exe.IsComplete)
            {
                machine.Execute(exe);
            }
        }

        public void Execute(Execution execution)
        {
            if (execution.Context.Ticks >= Constants.MAX_TICKS || execution.Context.Depth >= Constants.MAX_DEPTH)
            {
                throw new BoundaryException();
            }

            execution.Context.Ticks++;

            var op = execution.Next();
            if (!Operations.ContainsKey(op))
            {
                return; // NoOp
            }

            var next = Operations[op];
            if (next.Machine == this)
            {
                execution.Context.Depth++;
                var exe = new Execution(execution.Context, next.Data);
                Run(this, exe);
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

        public void Store(byte word, byte[] data)
        {
            if (word == 0)
            {
                return;
            }

            if (data == null || data.Length == 0)
            {
                Operations.Remove(word);
                return;
            }

            var exe = new Executable(this, null, data);
            Set(word, exe);
        }
    }
}
