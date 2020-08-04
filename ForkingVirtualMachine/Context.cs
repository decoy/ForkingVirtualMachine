namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Utility;
    using System;
    using System.Collections.Generic;

    public class Context : IContext
    {
        public readonly Stack<IVirtualMachine> Executions = new Stack<IVirtualMachine>();
        public readonly Stack<ReadOnlyMemory<byte>> Stack = new Stack<ReadOnlyMemory<byte>>();
        public readonly Store<ReadOnlyMemory<byte>> Definitions = new Store<ReadOnlyMemory<byte>>();

        public int Ticks { get; private set; }

        public IScope Caller { get; }
        public ICallScheduler Scheduler { get; }

        public Context(IScope caller, ICallScheduler scheduler)
        {
            Caller = caller;
            Scheduler = scheduler;
        }

        public void Tick()
        {
            if (Ticks == Constants.MAX_TICKS)
            {
                throw new BoundaryException();
            }
            Ticks++;
        }

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

        public void Call(IScope from, byte[] scopeId, ReadOnlyMemory<byte> data)
        {
            // needs that scheduler
            throw new NotImplementedException();
        }

        public void Define(byte[] word, ReadOnlyMemory<byte> data)
        {
            Definitions.Set(word, data);
        }
    }
}
