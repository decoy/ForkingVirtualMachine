namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class Context : IVirtualMachine
    {
        private IVirtualMachine parent;

        private Dictionary<byte, IEnumerable<byte>> dictionary = new Dictionary<byte, IEnumerable<byte>>();

        private Stack<long> stack = new Stack<long>();

        public Context(IVirtualMachine parent)
        {
            this.parent = parent;
        }

        public void Run(IEnumerator<byte> stream)
        {
            while (stream.TryNext(out var op))
            {
                Execute(this, op, stream);
            }
        }

        public void Execute(Context context, byte op, IEnumerator<byte> stream)
        {
            if (Has(op))
            {
                // if the first byte is 0, it's a local scope
                var code = Get(op).GetEnumerator();
                var scope = code.Next();

                if (scope == VirtualMachine.LocalScope)
                {
                    // TODO: unrecur this so we can save the execution context stack
                    Run(code);
                }
                else
                {
                    parent.Execute(context, op, stream);
                }
            }
            else
            {
                throw new UnknownOperationException(op);
            }
        }

        public IEnumerable<byte> Get(byte word)
        {
            return dictionary[word];
        }

        public bool Has(byte word)
        {
            return dictionary.ContainsKey(word);
        }

        public void Set(byte word, IEnumerable<byte> action)
        {
            if (dictionary.ContainsKey(word))
            {
                dictionary[word] = action;
            }
            else
            {
                dictionary.Add(word, action);
            }
        }

        public void Unset(byte word)
        {
            dictionary.Remove(word);
        }

        public void Push(long data)
        {
            stack.Push(data);
        }

        public long Pop()
        {
            return stack.Pop();
        }
    }
}
