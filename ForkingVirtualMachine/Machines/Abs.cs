﻿using System.Numerics;

namespace ForkingVirtualMachine.Machines
{
    public class Abs : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Abs();

        public void Execute(IScope scope, IContext context)
        {
            var a = context.PopInt();
            context.Push(BigInteger.Abs(a));
        }
    }
}
