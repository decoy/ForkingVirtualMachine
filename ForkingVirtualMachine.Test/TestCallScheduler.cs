namespace ForkingVirtualMachine.Test
{
    using System;
    using System.Collections.Generic;

    public class TestCallScheduler : ICallScheduler
    {
        public class Scheduled
        {
            public IScope Caller { get; set; }
            public byte[] ScopeId { get; set; }
            public ReadOnlyMemory<byte> Data { get; set; }
        }

        public readonly Queue<Scheduled> Queue = new Queue<Scheduled>();

        public void Schedule(IScope caller, byte[] scopeId, ReadOnlyMemory<byte> data)
        {
            Queue.Enqueue(new Scheduled()
            {
                Caller = caller,
                ScopeId = scopeId,
                Data = data,
            });
        }
    }
}
