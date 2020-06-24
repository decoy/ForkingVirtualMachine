namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;

    public class Context : IVirtualMachine
    {
        private IVirtualMachine parent;

        private Dictionary<byte, IEnumerable<byte>> dictionary = new Dictionary<byte, IEnumerable<byte>>();

        public Context(IVirtualMachine parent)
        {
            this.parent = parent;
        }

        public static Context Fork(IVirtualMachine parent, byte[] words)
        {
            var context = new Context(parent);
            foreach (var word in words)
            {
                context.Set(word, new byte[] { VirtualMachine.ParentScope });
            }
            return context;
        }

        public void Run(IEnumerable<byte> stream)
        {
            Run(stream.GetEnumerator(), new Stack<long>());
        }

        public void Run(IEnumerator<byte> stream, Stack<long> stack)
        {
            while (stream.TryNext(out var op))
            {
                Execute(this, op, stream, stack);
            }
        }

        public void Execute(Context context, byte op, IEnumerator<byte> stream, Stack<long> stack)
        {
            if (Has(op))
            {
                // if the first byte is 0, it's a local scope
                var code = Get(op).GetEnumerator();
                var scope = code.Next();

                if (scope == VirtualMachine.LocalScope)
                {
                    Run(code, stack);
                }
                else
                {
                    parent.Execute(context, op, stream, stack);
                }
            }
            else
            {
                throw new Exception("unknown:" + op);
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
    }
}
