namespace ForkingVirtualMachine.Test
{
    public class Op
    {
        public const byte No = Constants.SafeWord;
        public const byte Boom = Constants.SelfDestruct;

        public const byte Push = Constants.Push8;
        public const byte Push32 = Constants.Push32;
        public const byte Define = Constants.Define;

        public const byte Add = 20;
        public const byte Print = 30;

        public const byte NoOp = 40;

        public static class Math
        {
            public const byte Namespace = 50;
            public const byte Subtract = 1;
            public const byte DivRem = 2;
        }

        public const byte a = 100;
        public const byte b = 101;
        public const byte c = 102;
        public const byte d = 103;

        public const byte x = 200;
        public const byte y = 201;
        public const byte z = 202;
    }
}
