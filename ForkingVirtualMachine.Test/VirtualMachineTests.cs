using ForkingVirtualMachine.Extensions;
using ForkingVirtualMachine.Arithmetic;
using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ForkingVirtualMachine.Flow;
using System.Numerics;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class VirtualMachineTests
    {
        private static VirtualMachine CreateTestVm(Collector col)
        {
            var vm = new VirtualMachine();
            vm.Set(Reg.No, new Executable(SafeWord.Machine, null, null));
            vm.Set(Reg.Boom, new Executable(Boom.Machine, null, null));
            vm.Set(Reg.Push, new Executable(Push.Machine, null, null));
            vm.Set(Reg.Push32, new Executable(Push.Machine, null, null));
            vm.Set(Reg.Define, new Executable(new Define(vm), null, null));

            vm.Set(Reg.Add, new Executable(Add.Machine, null, null));
            vm.Set(Reg.Print, new Executable(col, null, null));

            var vm2 = new VirtualMachine();
            vm.Set(Reg.Math.Namespace, new Executable(vm2, null, null));

            vm2.Set(Reg.Math.Subtract, new Executable(Subtract.Machine, null, null));
            vm2.Set(Reg.Math.DivRem, new Executable(DivideRem.Machine, null, null));

            return vm;
        }

        [TestMethod]
        public void RunsFunction()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = new List<byte>()
                .Add(
                    Reg.NoOp,
                    Reg.Push
                )
                .AddData(
                    Reg.Push, 1, 100,
                    Reg.Add
                )
                .Add(
                    Reg.Push, 1, Reg.x,
                    Reg.Define,

                    Reg.Push, 1, 99,
                    Reg.Print,

                    Reg.Push, 1, 7,
                    Reg.x,
                    Reg.Print
                )
                .ToArray();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            Assert.AreEqual(99, col.Collected.Dequeue());
            Assert.AreEqual(107, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsMachine()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = new List<byte>()
                .Add(
                    Reg.Push, 1, 2,
                    Reg.Push, 1, 5,
                    Reg.Add,
                    Reg.Print
                )
                .ToArray();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsNamespace()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = new List<byte>()
                .Add(
                Reg.Push, 1, 2,
                    Reg.Push, 1, 5,
                    Reg.Push, 1, 7,

                    Reg.Math.Namespace,
                    Reg.Math.Subtract,

                    Reg.Print
                )
                .ToArray();

            VirtualMachine.Run(vm, new Execution(ctx, fun));

            Assert.AreEqual(2, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsSafeWord()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);
            var ctx = new Context();

            var fun = new List<byte>()
                .Add(
                    Reg.No,
                    Reg.Boom
                )
                .ToArray();

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

            var fun = new List<byte>()
                .Add(
                    Reg.Push, 1, Reg.y,
                    Reg.Push, 1, Reg.x,
                    Reg.Define,

                    Reg.Push, 1, Reg.x,
                    Reg.Push, 1, Reg.y,
                    Reg.Define,

                    Reg.x
                )
                .ToArray();

            Assert.ThrowsException<BoundaryException>(() =>
            {
                VirtualMachine.Run(vm, new Execution(ctx, fun));
            });
        }
    }
}
