namespace ForkingVirtualMachine.Store.Models
{
    using System;
    using System.Numerics;

    public class FeedChange
    {
        public byte[] NodeId { get; set; }

        // this is a stream: could be used instead of val on node table (immutable)
        public BigInteger Value { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
