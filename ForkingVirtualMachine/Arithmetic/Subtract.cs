﻿namespace ForkingVirtualMachine.Math
{
    public class Subtract : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Subtract();

        public void Execute(Context context)
        {
            context.Stack.Push(checked(context.Stack.Pop() - context.Stack.Pop()));
        }
    }
}
