﻿namespace ForkingVirtualMachine.Math
{
    public class Add : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Add();

        public void Execute(Context context)
        {
            context.Stack.Push(context.Stack.Pop() + context.Stack.Pop());
        }
    }
}
