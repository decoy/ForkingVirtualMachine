﻿namespace ForkingVirtualMachine.Machines
{
    public class GreaterThan : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new GreaterThan();

        public void Execute(IContext context)
        {
            var a = context.PopInt();
            var b = context.PopInt();

            context.Push(a > b
                ? Constants.True
                : Constants.False);
        }
    }
}
