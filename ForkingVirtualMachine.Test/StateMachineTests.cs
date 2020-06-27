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
            var ctx = new Context(new byte[]
            {
                word, 2, 9, 10, // define as 9, 10
                250,            // random op
                word, 0,        // undefine
                251,            // random op
                word, 0,
            });

            Define.Machine.Execute(ctx);

            // added the function
            Assert.AreEqual(1, ctx.Functions.Count);

            // the word is the correct length and has values
            var exe = ctx.Functions[word];
            Assert.AreEqual(2, exe.Data.Length);
            Assert.AreEqual(9, exe.Data.Span[0]);
            Assert.AreEqual(10, exe.Data.Span[1]);

            // the execution has been forwarded
            Assert.AreEqual(250, ctx.Next());

            // undefines
            Define.Machine.Execute(ctx);
            Assert.AreEqual(0, ctx.Functions.Count);
            Assert.AreEqual(251, ctx.Next());

            // current behavior is no error on undefining
            Define.Machine.Execute(ctx);
            Assert.IsTrue(ctx.IsComplete);
        }

        [TestMethod]
        public void Depths()
        {
            var ctx = new Context(null);
            ctx.Stack.Push(5);
            ctx.Stack.Push(9);

            Depth.Machine.Execute(ctx);

            Assert.AreEqual(2, ctx.Stack.Pop());
            Assert.AreEqual(9, ctx.Stack.Pop());
            Assert.AreEqual(5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void Drops()
        {
            var ctx = new Context(null);
            ctx.Stack.Push(5);
            ctx.Stack.Push(9);

            Drop.Machine.Execute(ctx);

            Assert.AreEqual(5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void Swaps()
        {
            var ctx = new Context(null);
            ctx.Stack.Push(5);
            ctx.Stack.Push(9);

            Swap.Machine.Execute(ctx);

            Assert.AreEqual(5, ctx.Stack.Pop());
            Assert.AreEqual(9, ctx.Stack.Pop());
        }

        [TestMethod]
        public void Dupes()
        {
            var ctx = new Context(null);
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
            var ctx = new Context(new byte[]
            {
                99, 100
            });
            ctx.Stack.Push(5);

            Push.Machine.Execute(ctx);

            Assert.AreEqual(99, ctx.Stack.Pop());
            Assert.AreEqual(5, ctx.Stack.Pop());
            Assert.AreEqual(100, ctx.Next());
        }

        [TestMethod]
        public void PushesN()
        {
            var ctx = new Context(new byte[]
            {
                2, 99, 100, 101
            });
            ctx.Stack.Push(5);

            PushN.Machine.Execute(ctx);

            Assert.AreEqual(100, ctx.Stack.Pop());
            Assert.AreEqual(99, ctx.Stack.Pop());
            Assert.AreEqual(5, ctx.Stack.Pop());
            Assert.AreEqual(101, ctx.Next());
        }

        [TestMethod]
        public void PushesInt64Big()
        {
            var a = new byte[8];
            var b = new byte[8];

            BinaryPrimitives.WriteInt64BigEndian(a, long.MaxValue);
            BinaryPrimitives.WriteInt64BigEndian(b, long.MinValue);

            var exe = a.Concat(b).ToArray();

            var ctx = new Context(exe);
            ctx.Stack.Push(5);

            PushInt64BigEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MaxValue, ctx.Stack.Pop());

            PushInt64BigEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MinValue, ctx.Stack.Pop());
            Assert.IsTrue(ctx.IsComplete);
        }

        [TestMethod]
        public void PushesInt64Little()
        {
            var a = new byte[8];
            var b = new byte[8];

            BinaryPrimitives.WriteInt64LittleEndian(a, long.MaxValue);
            BinaryPrimitives.WriteInt64LittleEndian(b, long.MinValue);

            var exe = a.Concat(b).ToArray();

            var ctx = new Context(exe);
            ctx.Stack.Push(5);

            PushInt64LittleEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MaxValue, ctx.Stack.Pop());

            PushInt64LittleEndian.Machine.Execute(ctx);
            Assert.AreEqual(long.MinValue, ctx.Stack.Pop());
            Assert.IsTrue(ctx.IsComplete);
        }
    }
}