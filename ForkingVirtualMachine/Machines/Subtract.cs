﻿namespace ForkingVirtualMachine.Machines
{
    public class Subtract : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Subtract();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();
            context.Push(a - b);
        }
    }
}
