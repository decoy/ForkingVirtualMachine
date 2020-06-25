namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class Context
    {
        public Dictionary<byte, Execution> Functions { get; } = new Dictionary<byte, Execution>();
        public Stack<long> Stack { get; } = new Stack<long>();
        public Stack<Execution> Executions { get; } = new Stack<Execution>();

        public Execution Execution => Executions.Peek();
    }
}
