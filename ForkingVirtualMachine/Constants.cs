namespace ForkingVirtualMachine
{
    public static class Constants
    {
        public static readonly byte[] One = new byte[1] { 1 };

        public static readonly byte[] Zero = new byte[1] { 0 };

        public static readonly byte[] Empty = new byte[0];

        public static readonly byte[] True = new byte[1] { byte.MaxValue };

        public static readonly byte[] False = Empty;

        public const int MAX_REGISTER_SIZE = 1024 * 1024;

        public const int MAX_STACK_DEPTH = 256;

        public const int MAX_EXE_DEPTH = 256;

        public const int MAX_TICKS = 1024;

        public const byte MAX_INT_BYTES = 16;

        public const byte DEFINE = 1;

        public const byte EXECUTE = 0;
    }
}
