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

        private static VirtualMachine2 CreateTestVm(Collector col)
        {
            return new VirtualMachine2()
                .Add(Op.Push, Push.Machine)
                .Add(Op.Add, Add.Machine)
                .Add(Op.Define, Define.Machine)
                .Add(Op.PushN, PushN.Machine)
                .Add(Op.Dupe, Dupe.Machine)
                .Add(Op.Print, col);
        }

        [TestMethod]
        public void RunsMachine()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);

            var program = new List<byte>()
                .AddProgram(Op.Push, 5, Op.Push, 2, Op.Add, Op.Print)
                .ToExecution();

            var ctx = vm.Fork();
            ctx.Executions.Push(program);

            vm.Run(ctx);

            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void TestyTestums()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);


            byte word = 240;

            var program = new List<byte>()
                // create a function (adds 100)
                .AddProgram(Op.Define, word)
                .AddData(Op.Push, 100, Op.Add)

                // Push 5 and 2 to the stack
                .AddProgram(Op.PushN)
                .AddData(5, 2)

                // add 5 + 2, dupe, print
                .AddProgram(Op.Add, Op.Dupe, Op.Print)

                // then run the program +100 funcion and print
                .AddProgram(word, Op.Print)
                .ToExecution();

            var ctx = vm.Fork();
            ctx.Executions.Push(program);

            vm.Run(ctx);

            Assert.AreEqual(5 + 2, col.Collected.Dequeue());
            Assert.AreEqual(5 + 2 + 100, col.Collected.Dequeue());

            var program2 = new List<byte>()
                .AddProgram(Op.Push, 99, word, Op.Print)
                .ToExecution();

            // FIXME - not routing internally correctly
            // routed should run on the parent context
            // I'm thinking exe's should have a 'scope' var that is just how far down it goes.
            // "0" would be the current way, yeah? (wondering if this is somehow a function, the pushexe)
            var ctx2 = ctx.Fork(word, Op.Print, Op.Push);
            ctx2.Executions.Push(program2);
            vm.Run(ctx2);

            Assert.AreEqual(99 + 100, col.Collected.Dequeue());
        }
    }
}
