namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public class Context
    {
        private readonly Stack<byte[]> Stack = new Stack<byte[]>();

        public int Ticks { get; set; }

        public int Depth { get; set; }

        public void Push(byte[] data)
        {
            if (data.Length > Constants.MAX_REGISTER_SIZE)
            {
                throw new BoundaryException();
            }

            if (Stack.Count == Constants.MAX_STACK_DEPTH)
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
