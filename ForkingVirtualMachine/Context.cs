namespace ForkingVirtualMachine
{
    using System;
    using System.Collections.Generic;

    public class Context
    {
        public readonly Stack<ReadOnlyMemory<byte>> Stack = new Stack<ReadOnlyMemory<byte>>();

        public readonly Stack<IVirtualMachine> Executions = new Stack<IVirtualMachine>();

        public int Ticks { get; set; }

        public void Push(IVirtualMachine exe)
        {
            if (Executions.Count == Constants.MAX_EXE_DEPTH)
            {
                throw new BoundaryException();
            }

            Executions.Push(exe);
        }

        public void Push(ReadOnlyMemory<byte> data)
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

        public ReadOnlyMemory<byte> Pop()
        {
            return Stack.Pop();
        }
    }
}
