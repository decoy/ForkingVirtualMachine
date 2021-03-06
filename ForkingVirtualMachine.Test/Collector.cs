﻿using System.Collections.Generic;
using System.Numerics;

namespace ForkingVirtualMachine.Test
{
    public class Collector : IVirtualMachine
    {
        public Queue<BigInteger> Collected { get; } = new Queue<BigInteger>();

        public void Execute(IScope scope, IContext context)
        {
            Collected.Enqueue(context.PopInt());
        }
    }
}
