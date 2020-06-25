using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Buffers.Binary;
using System.Linq;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class StateMachineTests
    {
        [TestMethod]
        public void Defines()
        {
            byte word = 240;
            var ctx = new Context();

            ctx.Executions.Push(new Execution(ctx, new byte[]
            {
                word, 2, 9, 10, // define as 9, 10
                250,            // random op
                word, 0,        // undefine
                251,            // random op
                word, 0,
            }));

            Define.Machine.Execute(ctx);

            // added the function
            Assert.AreEqual(1, ctx.Functions.Count);

            // the word is the correct length and has values
            var exe = ctx.Functions[word];
            Assert.AreEqual(2, exe.Length);
            Assert.AreEqual(9, exe.Next());
            Assert.AreEqual(10, exe.Next());

            // the execution has been forwarded
            Assert.AreEqual(250, ctx.Execution.Next());

            // undefines
            Define.Machine.Execute(ctx);
            Assert.AreEqual(0, ctx.Functions.Count);
            Assert.AreEqual(251, ctx.Execution.Next());

            // current behavior is no error on undefining
            Define.Machine.Execute(ctx);
            Assert.IsTrue(ctx.Execution.IsComplete);
        }

        [TestMethod]
        public void Dupes()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            Dupe.Machine.Execute(ctx);

            Assert.AreEqual(2, ctx.Stack.Pop());
            Assert.AreEqual(2, ctx.Stack.Pop());
            Assert.AreEqual(5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void Pushes()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);

            ctx.Executions.Push(new Execution(ctx, new byte[]
            {
                99, 100
            }));

            Push.Machine.Execute(ctx);

            Assert.AreEqual(99, ctx.Stack.Pop());
            Assert.AreEqual(5, ctx.Stack.Pop());
            Assert.AreEqual(100, ctx.Execution.Next());
        }

        [TestMethod]
        public void PushesN()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);

            ctx.Executions.Push(new Execution(ctx, new byte[]
            {
                2, 99, 100, 101
            }));

            PushN.Machine.Execute(ctx);

            Assert.AreEqual(100, ctx.Stack.Pop());
            Assert.AreEqual(99, ctx.Stack.Pop());
            Assert.AreEqual(5, ctx.Stack.Pop());
            Assert.AreEqual(101, ctx.Execution.Next());
        }

        [TestMethod]
        public void PushesInt64Big()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);

            var a = new byte[8];
            var b = new byte[8];

            BinaryPrimitives.WriteInt64BigEndian(a, long.MaxValue);
            BinaryPrimitives.WriteInt64BigEndian(b, long.MinValue);

            var exe = a.Concat(b).ToArray();

            ctx.Executions.Push(new Execution(ctx, exe));

            PushInt64BigEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MaxValue, ctx.Stack.Pop());

            PushInt64BigEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MinValue, ctx.Stack.Pop());
            Assert.IsTrue(ctx.Execution.IsComplete);
        }

        [TestMethod]
        public void PushesInt64Little()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);

            var a = new byte[8];
            var b = new byte[8];

            BinaryPrimitives.WriteInt64LittleEndian(a, long.MaxValue);
            BinaryPrimitives.WriteInt64LittleEndian(b, long.MinValue);

            var exe = a.Concat(b).ToArray();

            ctx.Executions.Push(new Execution(ctx, exe));

            PushInt64LittleEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MaxValue, ctx.Stack.Pop());

            PushInt64LittleEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MinValue, ctx.Stack.Pop());
            Assert.IsTrue(ctx.Execution.IsComplete);
        }
    }
}