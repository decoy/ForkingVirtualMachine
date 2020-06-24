namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public interface IVirtualMachine
    {
        public void Execute(Context context, byte op, IEnumerator<byte> stream);
    }
}
