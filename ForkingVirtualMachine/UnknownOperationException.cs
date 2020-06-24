namespace ForkingVirtualMachine
{
    using System;

    public class UnknownOperationException : Exception
    {
        public byte Operation { get; }
        public UnknownOperationException(byte operation) : base("Unknown operation: " + operation)
        {
            Operation = operation;
        }
    }
}
