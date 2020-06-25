﻿namespace ForkingVirtualMachine.State
{
    using System.Buffers.Binary;

    public class PushInt64LittleEndian : IVirtualMachine
    {
        public void Execute(Context context)
        {
            context.Stack.Push(BinaryPrimitives.ReadInt64LittleEndian(context.Execution.Next(8)));
        }
    }
}
