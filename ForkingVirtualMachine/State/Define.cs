namespace ForkingVirtualMachine.State
{
    public class Define : IVirtualMachine
    {
        public const byte Local = 0;

        public static readonly IVirtualMachine Machine = new Define();

        public void Execute(Context context)
        {
            var word = context.Next();
            var n = context.Next();

            if (n < 1) // undefine
            {
                context.Functions.Remove(word);
                return;
            }


            var exe = new Executable(null, null, context.Next(n).ToArray());

            if (context.Functions.ContainsKey(word))
            {
                context.Functions[word] = exe;
            }
            else
            {
                context.Functions.Add(word, exe);
            }
        }
    }
}
