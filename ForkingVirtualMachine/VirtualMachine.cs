namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;

    public class VirtualMachine : IVirtualMachine
    {
        public static class Operations
        {
            public const byte Push = 0;
            public const byte PushN = 1;
            public const byte Define = 2;
            public const byte Add = 20;
            public const byte Print = 100;
        }

        public const byte LocalScope = 0;
        public const byte ParentScope = 1;

        public void Execute(Context context, byte op, IEnumerator<byte> stream, Stack<long> stack)
        {
            switch (op)
            {
                // pushes next item to the stack
                case Operations.Push:
                    stack.Push(stream.Next());
                    break;

                // pushes the next n items to the stack
                case Operations.PushN:
                    var npush = stream.Next();
                    for (var i = 0; i < npush; i++) stack.Push(stream.Next());
                    break;

                // defines (or unsets) a word in the context
                case Operations.Define:
                    var word = stream.Next();
                    var ndef = stream.Next();

                    if (ndef < 1)
                    {
                        context.Unset(word);
                        break;
                    }

                    var data = new byte[ndef + 1];
                    data[0] = LocalScope;

                    for (var i = 1; i <= ndef; i++)
                    {
                        data[i] = stream.Next();
                    }
                    context.Set(word, data);
                    break;

                // adds the top two items on the stack
                case Operations.Add:
                    stack.Push(stack.Pop() + stack.Pop());
                    break;

                // pops and prints the top of the stack
                case Operations.Print:
                    Console.WriteLine(stack.Pop());
                    break;

                default:
                    throw new Exception("unknown:" + op);
            }
        }
    }
}
