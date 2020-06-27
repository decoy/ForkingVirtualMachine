namespace ForkingVirtualMachine.State
{
    using System.Buffers.Binary;

    public class PushInt64LittleEndian : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new PushInt64LittleEndian();

        public void Execute(Context context)
        {
            context.Stack.Push(BinaryPrimitives.ReadInt64LittleEndian(context.Next(8)));
        }
    }
}
