namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    // https://stackoverflow.com/questions/7244699/gethashcode-on-byte-array
    public sealed class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        private static readonly EqualityComparer<T> elementComparer
            = EqualityComparer<T>.Default;

        public bool Equals(T[] first, T[] second)
        {
            if (first == second)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            if (first.Length != second.Length)
            {
                return false;
            }
            for (int i = 0; i < first.Length; i++)
            {
                if (!elementComparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(T[] array)
        {
            unchecked
            {
                if (array == null)
                {
                    return 0;
                }
                int hash = 17;
                foreach (T element in array)
                {
                    hash = hash * 31 + elementComparer.GetHashCode(element);
                }
                return hash;
            }
        }
    }

    public class Store<T> : Dictionary<byte[], T>
    {
        public Store()
            : base(new ArrayEqualityComparer<byte>())
        { }

        public Store(IDictionary<byte[], T> dictionary)
            : base(dictionary, new ArrayEqualityComparer<byte>())
        { }
    }
}
