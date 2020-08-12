namespace ForkingVirtualMachine.Store.Models
{
    using System;
    using System.Numerics;

    public class FeedChange
    {
        public byte[] Id { get; set; }

        public byte[] ParentId { get; set; }

        public byte[] DataId { get; set; }

        public BigInteger Weight { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int Version { get; set; }
    }
}
