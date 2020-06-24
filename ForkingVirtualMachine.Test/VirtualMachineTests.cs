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
            var ctx = vm.Fork(new byte[] { VirtualMachine.Operations.Push, VirtualMachine.Operations.Print, VirtualMachine.Operations.Define });

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

            ctx.Run(program);

            var ctx2 = ctx.Fork(new byte[] { 240 });

            ctx2.Run(new byte[] { 240 });
        }
    }
}
