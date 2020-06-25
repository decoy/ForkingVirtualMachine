namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class Context
    {
        public Context Parent { get; }

        public Context Scope { get; private set; }

        public Dictionary<byte, Execution> Functions { get; } = new Dictionary<byte, Execution>();
        public Stack<long> Stack { get; } = new Stack<long>();
        public Stack<Execution> Executions { get; } = new Stack<Execution>();

        public Execution Execution => Executions.Peek();

        public Context()
        {
            Scope = this;
        }

        public Context(Context parent) : base()
        {
            Parent = parent;
        }

        // add scoped
        // pop scoped
    }
}
