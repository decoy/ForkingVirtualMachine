namespace ForkingVirtualMachine
{
    using ForkingVirtualMachine.Utility;
    using System;
    using System.Collections.Generic;

    public class Context : IContext
    {
        public readonly Stack<IExecution> Executions = new Stack<IExecution>();
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

        public void Push(IExecution exe)
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

        public bool Pop(out IExecution execution)
        {
            return Executions.TryPop(out execution);
        }

        public void Call(IScope from, byte[] scopeId, ReadOnlyMemory<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}
