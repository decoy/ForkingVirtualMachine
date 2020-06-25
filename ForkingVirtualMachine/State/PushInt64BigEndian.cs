namespace ForkingVirtualMachine.State
{
    using System.Buffers.Binary;

    public class PushInt64BigEndian : IVirtualMachine
    {
        public void Execute(Context context)
        {
            context.Stack.Push(BinaryPrimitives.ReadInt64BigEndian(context.Execution.Next(8)));
        }
    }
}
