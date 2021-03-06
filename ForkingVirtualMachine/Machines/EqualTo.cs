﻿namespace ForkingVirtualMachine.Machines
{
    public class EqualTo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new EqualTo();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a == b);
        }
    }
}
