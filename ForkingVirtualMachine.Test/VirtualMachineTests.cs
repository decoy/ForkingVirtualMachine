using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class VirtualMachineTests
    {
        [TestMethod]
        public void TestStuff()
        {
            // quick hack smoke test.

            var vm = new VirtualMachine();
            var ctx = Context.Fork(vm, new byte[] { VirtualMachine.Operations.Push, VirtualMachine.Operations.Print, VirtualMachine.Operations.Define });

            var subprogram = new byte[]
            {
                VirtualMachine.Operations.Push, 3,
                VirtualMachine.Operations.Print
            };

            var program = new List<byte>();
            program.AddRange(new byte[] { VirtualMachine.Operations.Define, 240, (byte)subprogram.Length });
            program.AddRange(subprogram);
            program.AddRange(new byte[] { VirtualMachine.Operations.Push, 111, VirtualMachine.Operations.Print });

            program.Add(240);

            // should... should the stack be part of the context? like the dictionary?
            // a 'saved' program might depend on it?
            // and a dictionary can modify as it goes too... (executions... shit.)
            var stack = new Stack<long>();

            ctx.Run(program.GetEnumerator(), stack);

            var ctx2 = Context.Fork(ctx, new byte[] { 240 });

            ctx.Run(new List<byte>(new byte[] { 240 }).GetEnumerator(), stack);
        }
    }
}
