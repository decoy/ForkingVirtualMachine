using ForkingVirtualMachine.Arithmetic;
using ForkingVirtualMachine.Flow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class VirtualMachineTests
    {
        private static void Run(Context ctx)
        {
            while (ctx.Executions.Count > 0)
            {
                ctx.Executions.Pop().Execute(ctx);
            }
        }

        private static Context CreateContext(Collector col, byte[] data)
        {
            var machines = new Dictionary<BigInteger, IVirtualMachine>();
            var loader = new Load(machines);

            machines.Add(Op.Push, new Define(loader, machines));
            machines.Add(Op.Add, Add.Machine);
            machines.Add(Op.Print, col);

            var ctx = new Context();
            ctx.Push(new VirtualMachine(loader, data));

            return ctx;
        }

        [TestMethod]
        public void RunsMachine()
        {
            var fun = ProgramBuilder.Create()
                   .Add(
                        1, 2,
                        1, 5,
                        1, Op.Add, 0,
                        1, Op.Print, 0
                   )
                   .ToBytes();

            var col = new Collector();

            Run(CreateContext(col, fun));

            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsFunction()
        {
            var fun = ProgramBuilder.Create()
                .Define(Op.x, (p) => p
                    .Push(1000)
                    .Execute(Op.Add)
                 )
                .Push(long.MaxValue)
                .Execute(Op.Print)
                .Push(99)
                .Execute(Op.Print)
                .Push(7)
                .Execute(Op.x, Op.Print)
                .ToBytes();

            var col = new Collector();

            Run(CreateContext(col, fun));

            Assert.AreEqual(long.MaxValue, col.Collected.Dequeue());
            Assert.AreEqual(99, col.Collected.Dequeue());
            Assert.AreEqual(1007, col.Collected.Dequeue());
        }



        //    [TestMethod]
        //    public void RunsNamespace()
        //    {
        //        var col = new Collector();
        //        var vm = CreateTestVm(col);
        //        var ctx = new Context();

        //        var fun = ProgramBuilder.Create()
        //            .Add(
        //                Op.Push, 1, 5,
        //                Op.Push, 1, 7,

        //                Op.Math.Namespace,
        //                Op.Math.Subtract,

        //                Op.Print
        //            )
        //            .ToBytes();

        //        VirtualMachine.Run(vm, new Execution(ctx, fun));

        //        Assert.AreEqual(2, col.Collected.Dequeue());
        //    }

        //    [TestMethod]
        //    public void RunsRedirected()
        //    {
        //        var col = new Collector();
        //        var vm = CreateTestVm(col);
        //        var ctx = new Context();

        //        var fun = ProgramBuilder.Create()
        //            .Define(Op.x,
        //                Op.Push, 1, 100,
        //                Op.Add
        //            )
        //            .ToBytes();

        //        VirtualMachine.Run(vm, new Execution(ctx, fun));

        //        // fork it
        //        var vm2 = new VirtualMachine();
        //        vm2.Set(Op.Push, vm.Operations[Op.Push]);
        //        vm2.Set(Op.Print, vm.Operations[Op.Print]);
        //        vm2.Set(Op.z, vm.Operations[Op.x]);

        //        var fun2 = ProgramBuilder.Create()
        //           .Add(
        //               Op.Push, 1, 5,
        //               Op.z,
        //               Op.Print,

        //               Op.Push, 1, 10,
        //               Op.Add,
        //               Op.Print
        //           )
        //           .ToBytes();

        //        var ctx2 = new Context();
        //        VirtualMachine.Run(vm2, new Execution(ctx2, fun2));

        //        Assert.AreEqual(105, col.Collected.Dequeue());

        //        // add misses
        //        Assert.AreEqual(10, col.Collected.Dequeue());
        //    }

        //    [TestMethod]
        //    public void RunsSafeWord()
        //    {
        //        var col = new Collector();
        //        var vm = CreateTestVm(col);
        //        var ctx = new Context();

        //        var fun = ProgramBuilder.Create()
        //            .Add(
        //                Op.No,
        //                Op.Boom
        //            )
        //            .ToBytes();

        //        var exe = new Execution(ctx, fun);
        //        VirtualMachine.Run(vm, exe);

        //        Assert.IsTrue(exe.IsComplete);
        //        Assert.IsTrue(exe.IsStopped);
        //    }

        //    [TestMethod]
        //    public void LimitsDepth()
        //    {
        //        var col = new Collector();
        //        var vm = CreateTestVm(col);
        //        var ctx = new Context();

        //        var fun = ProgramBuilder.Create()
        //            .Add(
        //                Op.Push, 1, Op.y,
        //                Op.Push, 1, Op.x,
        //                Op.Define,

        //                Op.Push, 1, Op.x,
        //                Op.Push, 1, Op.y,
        //                Op.Define,

        //                Op.x
        //            )
        //            .ToBytes();

        //        Assert.ThrowsException<BoundaryException>(() =>
        //        {
        //            VirtualMachine.Run(vm, new Execution(ctx, fun));
        //        });

        //        Assert.AreEqual(Constants.MAX_DEPTH, ctx.Depth);
        //    }

        //    [TestMethod]
        //    public void LimitsTicks()
        //    {
        //        var col = new Collector();
        //        var vm = CreateTestVm(col);
        //        var ctx = new Context();

        //        var noop = new Span<byte>(new byte[Constants.MAX_TICKS + 1]);
        //        noop.Fill(Op.NoOp);
        //        var fun = noop.ToArray();

        //        Assert.ThrowsException<BoundaryException>(() =>
        //        {
        //            VirtualMachine.Run(vm, new Execution(ctx, fun));
        //        });

        //        Assert.AreEqual(Constants.MAX_TICKS, ctx.Ticks);
        //    }
    }
}
