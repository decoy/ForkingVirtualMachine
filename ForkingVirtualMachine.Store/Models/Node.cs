namespace ForkingVirtualMachine.Store.Models
{
    using System;
    using System.Numerics;

    public class Node
    {
        public byte[] Id { get; set; }

        public byte[] FromId { get; set; }

        public byte[] ToId { get; set; }

        public byte[] DataId { get; set; }

        public bool Sign { get; set; }

        public BigInteger Weight { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int Version { get; set; }
    }
}
