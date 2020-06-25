using ForkingVirtualMachine.Extra;
using ForkingVirtualMachine.Math;
using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using ForkingVirtualMachine.Extensions;

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
            public const byte Dupe = 22;
        }

        public class Collector : IVirtualMachine
        {
            public Queue<long> Collected { get; } = new Queue<long>();

            public void Execute(Context context)
            {
                Collected.Enqueue(context.Stack.Pop());
            }
        }

        [TestMethod]
        public void TestyTestums()
        {
            var col = new Collector();
            var vm = new VirtualMachine()
                .Add(Op.Push, Push.Machine)
                .Add(Op.Add, Add.Machine)
                .Add(Op.Define, Define.Machine)
                .Add(Op.PushN, PushN.Machine)
                .Add(Op.Dupe, Dupe.Machine)
                .Add(Op.Print, col);

            var ctx = vm.Fork(vm.Machines.Keys.ToArray());

            byte word = 240;
            var subprogram = new byte[]
            {
                Op.Push, 100,
                Op.Add
            };

            var program = new List<byte>()
                .AddProgram(Op.Define, word)
                .AddData(subprogram)

                .AddProgram(Op.PushN)
                .AddData(5, 2)

                // add 5 + 2, dupe, print and pop
                .AddProgram(Op.Add, Op.Dupe, Op.Print)

                // then run the program which +100
                .AddProgram(word, Op.Print)
                .ToExecution();

            ctx.Run(program);

            Assert.AreEqual(5 + 2, col.Collected.Dequeue());
            Assert.AreEqual(5 + 2 + 100, col.Collected.Dequeue());

            var program2 = new List<byte>()
                .AddProgram(Op.Push, 99, word, Op.Print)
                .ToExecution();

            // FIXME - not routing internally correctly
            ctx.Fork(word, Op.Print, Op.Push).Run(program2);

            Assert.AreEqual(99 + 100, col.Collected.Dequeue());
        }
    }
}
