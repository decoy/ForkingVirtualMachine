namespace ForkingVirtualMachine.State
{
    public class Define : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Define();

        public void Execute(Context context)
        {
            var word = context.Next();

            var len = context.Next();

            if (len == 0)
            {
                context.Machine.Store(word, null);
                return;
            }

            var data = context.Next(len);
            context.Machine.Store(word, data.ToArray());
        }
    }
}
