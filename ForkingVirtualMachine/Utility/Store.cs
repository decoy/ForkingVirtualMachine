namespace ForkingVirtualMachine.Utility
{
    using System.Collections.Generic;

    public class Store<T> : Dictionary<byte[], T>
    {
        public Store() : base(new ArrayEqualityComparer<byte>()) { }

        public Store(Dictionary<byte[], T> store) : base(store, new ArrayEqualityComparer<byte>()) { }
    }
}
