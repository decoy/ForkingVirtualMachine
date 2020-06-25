using ForkingVirtualMachine.Extra;
using ForkingVirtualMachine.Math;
using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
            vm.Machines.Add(Op.Add, new Add());
            vm.Machines.Add(Op.Define, Define.Machine);
            vm.Machines.Add(Op.PushN, new PushN());
            vm.Machines.Add(Op.Print, new Print());

            var ctx = vm.Fork(vm.Machines.Keys.ToArray());

            byte word = 240;
            var subprogram = new byte[]
            {
                Op.Push, 99,
                Op.Print
            };

            var program = new List<byte>();
            program.AddRange(new byte[] { Op.Define, word, (byte)subprogram.Length });
            program.AddRange(subprogram);
            program.AddRange(new byte[] { Op.Push, 111, Op.Print });
            program.Add(word);

            ctx.Run(program);
        }
    }
}
