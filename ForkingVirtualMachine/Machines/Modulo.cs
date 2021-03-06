﻿namespace ForkingVirtualMachine.Machines
{
    public class Modulo : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Modulo();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a % b);
        }
    }
}
