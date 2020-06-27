namespace ForkingVirtualMachine.State
{
    using System.Buffers.Binary;

    public class PushInt64BigEndian : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new PushInt64BigEndian();

        public void Execute(Context context)
        {
            context.Stack.Push(BinaryPrimitives.ReadInt64BigEndian(context.Next(8)));
        }
    }
}
