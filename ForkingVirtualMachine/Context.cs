namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class Context
    {
        private readonly Stack<byte[]> Stack = new Stack<byte[]>(256);

        public int Ticks { get; set; }

        public int Depth { get; set; } // really tempted to just make another stack

        public void Push(byte[] data)
        {
            if (data.Length > Constants.MAX_REGISTER_SIZE)
            {
                throw new BoundaryException();
            }

            Stack.Push(data);
        }

        public byte[] Pop()
        {
            return Stack.Pop();
        }
    }
}
