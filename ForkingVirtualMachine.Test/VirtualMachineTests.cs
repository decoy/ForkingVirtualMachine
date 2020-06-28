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
            vm.Set(Reg.Define, new Executable(Define.Machine, null, null));

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

            var fun = new List<byte>()
                .Add(
                    Reg.Define, Reg.x
                )
                .AddData(
                    Reg.Define, Reg.a, 1, 5,
                    Reg.Define, Reg.b, 1, 2,
                    Reg.Add, Reg.a, Reg.b, Reg.c
                )
                .Add(
                    Reg.Print, Reg.c,
                    Reg.x,
                    Reg.Print, Reg.c
                )
                .ToArray();

            VirtualMachine.Run(vm, new Context(vm, fun));

            Assert.AreEqual(0, col.Collected.Dequeue());
            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsMachine()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);

            var fun = new List<byte>()
                .Add(
                    Reg.Define, Reg.a, 1, 5,
                    Reg.Define, Reg.b, 1, 2,
                    Reg.Add, Reg.a, Reg.b, Reg.c,
                    Reg.Print, Reg.c
                )
                .ToArray();

            VirtualMachine.Run(vm, new Context(vm, fun));

            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsNamespace()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);

            var fun = new List<byte>()
                .Add(
                    Reg.Define, Reg.a, 1, 5,
                    Reg.Define, Reg.b, 1, 2,
                    Reg.Math.Namespace, Reg.Math.Subtract, Reg.a, Reg.b, Reg.c,
                    Reg.Print, Reg.c
                )
                .ToArray();

            VirtualMachine.Run(vm, new Context(vm, fun));

            Assert.AreEqual(3, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsSafeWord()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);

            var fun = new List<byte>()
                .Add(
                    Reg.Define, Reg.a, 1, 5,
                    Reg.Define, Reg.b, 1, 2,
                    Reg.No,
                    Reg.Add, Reg.a, Reg.b, Reg.c,
                    Reg.Print, Reg.c
                )
                .ToArray();

            Assert.ThrowsException<SafeWordException>(() =>
            {
                VirtualMachine.Run(vm, new Context(vm, fun));
            });

            Assert.AreEqual(0, col.Collected.Count);
        }

        [TestMethod]
        public void RunsBoom()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);

            var fun = new List<byte>()
                .Add(
                    Reg.Define, Reg.a, 1, 5,
                    Reg.Define, Reg.b, 1, 2,
                    Reg.Boom,
                    Reg.Add, Reg.a, Reg.b, Reg.c,
                    Reg.Print, Reg.c
                )
                .ToArray();

            Assert.ThrowsException<SelfDestructException>(() =>
            {
                VirtualMachine.Run(vm, new Context(vm, fun));
            });

            Assert.AreEqual(0, col.Collected.Count);
        }

        [TestMethod]
        public void LimitsDepth()
        {
            var col = new Collector();
            var vm = CreateTestVm(col);

            var fun = new List<byte>()
                .Add(
                    Reg.Define, Reg.a, 1, Reg.b,
                    Reg.Define, Reg.b, 1, Reg.a,
                    Reg.a, // goto 1
                    Reg.Print, Reg.a
                )
                .ToArray();

            Assert.ThrowsException<BoundaryException>(() =>
            {
                VirtualMachine.Run(vm, new Context(vm, fun));
            });

            Assert.AreEqual(0, col.Collected.Count);
        }
    }
}
