namespace ForkingVirtualMachine.State
{
    using System;

    public class Define32 : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Define32();

        public void Execute(Context context)
        {
            var word = context.Next();

            var len = BitConverter.ToInt32(context.Next(4));

            if (len <= 0)
            {
                context.Machine.Store(word, null);
                return;
            }

            var data = context.Next(len);
            context.Machine.Store(word, data.ToArray());
        }
    }
}
