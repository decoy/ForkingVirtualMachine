using ForkingVirtualMachine.Arithmetic;
using ForkingVirtualMachine.Flow;
using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class VirtualMachineTests
    {
        private static VirtualMachine CreateTestVm(Collector col)
        {
            var vm = new VirtualMachine();
            vm.Set(Op.No, new Executable(SafeWord.Machine, null, null));
            vm.Set(Op.Boom, new Executable(Boom.Machine, null, null));
            vm.Set(Op.Push, new Executable(Push8.Machine, null, null));
            vm.Set(Op.Push32, new Executable(Push8.Machine, null, null));
            vm.Set(Op.Define, new Executable(new Define(vm), null, null));

            vm.Set(Op.Add, new Executable(Add.Machine, null, null));
            vm.Set(Op.Print, new Executable(col, null, null));

            var vm2 = new VirtualMachine();
            vm.Set(Op.Math.Namespace, new Executable(vm2, null, null));

            vm2.Set(Op.Math.Subtract, new Executable(Subtract.Machine, null, null));
            vm2.Set(Op.Math.DivRem, new Executable(DivideRem.Machine, null, null));

            return vm;
        }

        [TestMethod]
        public void RunsFunction()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = ProgramBuilder.Create()
                .Define(Op.x, (p) => p
                    .Push(1000)
                    .Add(Op.Add)
                 )
                .Push(long.MaxValue)
                .Add(Op.Print)
                .Add(
                    Op.Push, 1, 99,
                    Op.Print,

                    Op.Push, 1, 7,
                    Op.x,
                    Op.Print
                )
                .ToBytes();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            Assert.AreEqual(long.MaxValue, col.Collected.Dequeue());
            Assert.AreEqual(99, col.Collected.Dequeue());
            Assert.AreEqual(1007, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsMachine()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = ProgramBuilder.Create()
                .Add(
                    Op.Push, 1, 2,
                    Op.Push, 1, 5,
                    Op.Add,
                    Op.Print
                )
                .ToBytes();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsNamespace()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = ProgramBuilder.Create()
                .Add(
                    Op.Push, 1, 5,
                    Op.Push, 1, 7,

                    Op.Math.Namespace,
                    Op.Math.Subtract,

                    Op.Print
                )
                .ToBytes();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            Assert.AreEqual(2, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsRedirected()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = ProgramBuilder.Create()
                .Define(Op.x,
                    Op.Push, 1, 100,
                    Op.Add
                )
                .ToBytes();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            // fork it
            var vm2 = new VirtualMachine();
            vm2.Set(Op.Push, vm.Operations[Op.Push]);
            vm2.Set(Op.Print, vm.Operations[Op.Print]);
            vm2.Set(Op.z, vm.Operations[Op.x]);

            var fun2 = ProgramBuilder.Create()
               .Add(
                   Op.Push, 1, 5,
                   Op.z,
                   Op.Print,

                   Op.Push, 1, 10,
                   Op.Add,
                   Op.Print
               )
               .ToBytes();

            var ctx2 = new Context();
            VirtualMachine.Run(vm2, new Execution(ctx2, fun2));

            Assert.AreEqual(105, col.Collected.Dequeue());

            // add misses
            Assert.AreEqual(10, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsSafeWord()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = ProgramBuilder.Create()
                .Add(
                    Op.No,
                    Op.Boom
                )
                .ToBytes();

            var exe = new Execution(ctx, fun);
            VirtualMachine.Run(vm, exe);

            Assert.IsTrue(exe.IsComplete);
            Assert.IsTrue(exe.IsStopped);
        }

        [TestMethod]
        public void LimitsDepth()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = ProgramBuilder.Create()
                .Add(
                    Op.Push, 1, Op.y,
                    Op.Push, 1, Op.x,
                    Op.Define,

                    Op.Push, 1, Op.x,
                    Op.Push, 1, Op.y,
                    Op.Define,

                    Op.x
                )
                .ToBytes();

            Assert.ThrowsException<BoundaryException>(() =>
            {
                VirtualMachine.Run(vm, new Execution(ctx, fun));
            });

            Assert.AreEqual(Constants.MAX_DEPTH, ctx.Depth);
        }

        [TestMethod]
        public void LimitsTicks()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var noop = new Span<byte>(new byte[Constants.MAX_TICKS + 1]);
            noop.Fill(Op.NoOp);
            var fun = noop.ToArray();

            Assert.ThrowsException<BoundaryException>(() =>
            {
                VirtualMachine.Run(vm, new Execution(ctx, fun));
            });

            Assert.AreEqual(Constants.MAX_TICKS, ctx.Ticks);
        }
    }
}
