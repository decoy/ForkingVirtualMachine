using ForkingVirtualMachine.Machines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            scope.Set(Op.Define, define);
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
            var fun = ProgramBuilder.Create(p => p
                .Define(Op.x, d => d.Execute(Op.x))
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
