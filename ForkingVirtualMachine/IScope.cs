namespace ForkingVirtualMachine
{
    interface IScope : IDescribe
    {
        public void Define(byte[] word, IVirtualMachine machine);
        public void Call(byte[] scopeId, byte[] word, Context context);
    }
}
