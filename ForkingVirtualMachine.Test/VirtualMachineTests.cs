using ForkingVirtualMachine.Machines;
using ForkingVirtualMachine.Extra;
using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class VirtualMachineTests
    {
        public static class Op
        {
            public const byte Push = 1;
            public const byte PushN = 2;
            public const byte Define = 3;
            public const byte Add = 20;
            public const byte Print = 100;
        }

        [TestMethod]
        public void TestyTestums()
        {
            var vm = new VirtualMachine();
            vm.Machines.Add(Op.Push, new Push());
            vm.Machines.Add(Op.Define, Define.Machine);
            vm.Machines.Add(Op.PushN, new PushN());
            vm.Machines.Add(Op.Print, new Print());

            var ctx = new Context();
            ctx.Executions.Push(new Execution(new byte[]
            {
                0,
                Op.Push, 5,
                Op.Push, 6,
                Op.Add,
                Op.Print,
            }));

            vm.Execute(ctx);
        }

        [TestMethod]
        public void TestStuff()
        {
            // quick hack smoke test.

            //var vm = new VirtualMachine();
            //var ctx = vm.Fork(new byte[] { VirtualMachine.Operations.Push, VirtualMachine.Operations.Print, VirtualMachine.Operations.Define });

            //var subprogram = new byte[]
            //{
            //    VirtualMachine.Operations.Push, 3,
            //    VirtualMachine.Operations.Print
            //};

            //var program = new List<byte>();
            //program.AddRange(new byte[] { VirtualMachine.Operations.Define, 240, (byte)subprogram.Length });
            //program.AddRange(subprogram);
            //program.AddRange(new byte[] { VirtualMachine.Operations.Push, 111, VirtualMachine.Operations.Print });

            //program.Add(240);

            //ctx.Run(program);

            //var ctx2 = ctx.Fork(new byte[] { 240 });

            //ctx2.Run(new byte[] { 240 });
        }
    }
}
