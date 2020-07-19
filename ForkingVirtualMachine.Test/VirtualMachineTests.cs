using ForkingVirtualMachine.Machines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class VirtualMachineTests
    {
        private static Context Run(Context context)
        {
            while (context.Executions.Count > 0)
            {
                context.Executions.Pop().Execute(context);
            }
            return context;
        }

        private static Context CreateContext(Collector col, byte[] data)
        {
            var scope = new Scope(null);
            var define = new Define(scope);

            scope.Set(Op.Push, define);
            scope.Set(Op.Add, Add.Machine);
            scope.Set(Op.Print, col);
            scope.Set(Op.NoOp, NoOp.Machine);

            var ctx = new Context();
            ctx.Push(new Execution(define, data));

            return ctx;
        }

        [TestMethod]
        public void RunsMachine()
        {
            var fun = ProgramBuilder.Create(p => p
                .Write(new byte[] {
                    1, 2,
                    1, 5,
                    1, Op.Add, 0,
                    1, Op.Print, 0
                }));

            var col = new Collector();

            Run(CreateContext(col, fun));

            Assert.AreEqual(7, col.Collected.Dequeue());
        }

        [TestMethod]
        public void RunsFunction()
        {
            var fun = ProgramBuilder.Create(p => p
                .Define(Op.x, (d) => d
                    .Push(1000)
                    .Execute(Op.Add)
                 )
                .Push(long.MaxValue)
                .Execute(Op.Print)
                .Push(99)
                .Execute(Op.Print)
                .Push(7)
                .Execute(Op.x)
                .Execute(Op.Print));

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

        [TestMethod]
        public void LimitsDataStack()
        {
            var fun = ProgramBuilder.Create(p =>
            {
                for (var i = 0; i < Constants.MAX_STACK_DEPTH + 1; i++)
                {
                    p.Push(5);
                }
            });

            var col = new Collector();
            var ctx = CreateContext(col, fun);

            Assert.ThrowsException<BoundaryException>(() =>
            {
                Run(ctx);
            });

            Assert.AreEqual(Constants.MAX_STACK_DEPTH, ctx.Stack.Count);
        }

        [TestMethod]
        public void LimitsExeStack()
        {
            // going to have to rebuild this recurs
            var fun = ProgramBuilder.Create(p => p
                .Define(Op.x, d => d.Execute(Op.y))
                .Define(Op.y, d => d.Execute(Op.x))
                .Execute(Op.x));

            var col = new Collector();
            var ctx = CreateContext(col, fun);

            Assert.ThrowsException<BoundaryException>(() =>
            {
                Run(ctx);
            });

            Assert.AreEqual(Constants.MAX_EXE_DEPTH, ctx.Executions.Count);
        }

        [TestMethod]
        public void LimitsTicks()
        {
            var fun = ProgramBuilder.Create(p =>
            {
                for (var i = 0; i < Constants.MAX_TICKS + 1; i++)
                {
                    p.Execute(Op.NoOp);
                }
            });

            var col = new Collector();
            var ctx = CreateContext(col, fun);

            Assert.ThrowsException<BoundaryException>(() =>
            {
                Run(ctx);
            });

            Assert.AreEqual(Constants.MAX_TICKS, ctx.Ticks);
        }
    }
}
