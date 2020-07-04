namespace ForkingVirtualMachine.Store.Models
{
    using System;
    using System.Numerics;

    public class Node
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Type { get; set; }

        public byte[] Label { get; set; }

        public BigInteger Weight { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int Version { get; set; }
    }
}
