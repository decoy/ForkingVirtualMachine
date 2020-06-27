namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class Context
    {
        public readonly Dictionary<byte, Executable> Functions = new Dictionary<byte, Executable>();
        public readonly Stack<long> Stack = new Stack<long>();
        public readonly Execution Execution;

        public Context(Execution exe)
        {
            Execution = exe;
        }
    }
}
