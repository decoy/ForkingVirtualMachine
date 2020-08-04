namespace ForkingVirtualMachine.Utility
{
    using System;

    public static class ByteExtensions
    {
        private static readonly ArrayEqualityComparer<byte> compare = new ArrayEqualityComparer<byte>();

        public static bool IsEqual(this byte[] left, byte[] right)
        {
            return compare.Equals(left, right);
        }

        public static bool IsEqual(this byte[] left, byte right)
        {
            if (left.Length != 1) return false;
            return left[0] == right;
        }

        public static bool IsZero(this byte[] left)
        {
            for (var i = 0; i < left.Length; i++)
            {
                if (left[i] != 0) return false;
            }
            return true;
        }

        public static bool IsZero(this ReadOnlySpan<byte> left)
        {
            for (var i = 0; i < left.Length; i++)
            {
                if (left[i] != 0) return false;
            }
            return true;
        }
    }
}
