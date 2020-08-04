﻿namespace ForkingVirtualMachine.Machines
{
    public class Divide : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Divide();

        public void Execute(IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();
            context.Push(a / b);
        }
    }
}
