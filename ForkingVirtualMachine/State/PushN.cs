﻿namespace ForkingVirtualMachine.State
{
    public class PushN : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new PushN();

        public void Execute(Context context)
        {
            var n = context.Next();
            for (var i = 0; i < n; i++)
            {
                context.Stack.Push(context.Next());
            }
        }
    }
}
