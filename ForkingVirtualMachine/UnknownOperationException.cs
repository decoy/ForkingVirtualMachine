namespace ForkingVirtualMachine
{
    using System;

    // TODO: full op 'stack' in exception would be nice at this stage.
    // could probably build a try/catch thing around them to add them back in?
    public class UnknownOperationException : Exception
    {
        public byte Operation { get; }

        public UnknownOperationException(byte operation) : base("Unknown operation: " + operation)
        {
            Operation = operation;
        }
    }
}
