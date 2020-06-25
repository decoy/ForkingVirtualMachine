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

            var data = new byte[n];

            for (var i = 0; i < n; i++)
            {
                data[i] = context.Execution.Next();
            }

            var exe = new Execution(context, data);

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
