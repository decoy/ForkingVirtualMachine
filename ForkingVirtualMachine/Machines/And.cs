﻿namespace ForkingVirtualMachine.Machines
{
    public class And : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new And();

        public void Execute(IContext context)
        {
            var a = context.PopBool();
            var b = context.PopBool();
            context.Push(a && b);
        }
    }
}
