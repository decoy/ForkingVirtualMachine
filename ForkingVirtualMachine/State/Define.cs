namespace ForkingVirtualMachine.State
{
    public class Define : IVirtualMachine
    {
        public const byte Local = 0;

        public static readonly IVirtualMachine Machine = new Define();

        public void Execute(Context context)
        {
            var word = context.Execution.Next();
            var n = context.Execution.Next();

            if (n < 1) // undefine
            {
                context.Functions.Remove(word);
                return;
            }


            var exe = new Executable(null, null, context.Execution.Next(n).ToArray());

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
