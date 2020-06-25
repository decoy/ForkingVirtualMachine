using ForkingVirtualMachine.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class MathMachineTests
    {
        [TestMethod]
        public void Adds()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + 5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void AddsNegatives()
        {
            var ctx = new Context();
            ctx.Stack.Push(-5);
            ctx.Stack.Push(2);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + -5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void AddDoesNotOverflows()
        {
            var ctx = new Context();
            ctx.Stack.Push(long.MaxValue);
            ctx.Stack.Push(long.MaxValue);

            Assert.ThrowsException<OverflowException>(() => Add.Machine.Execute(ctx));
        }

        [TestMethod]
        public void Divides()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            Divide.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void DividesWithRemainder()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            DivideRem.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.Stack.Pop());
            Assert.AreEqual(2 % 5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void Modulos()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            Modulo.Machine.Execute(ctx);

            Assert.AreEqual(2 % 5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void Multiplies()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            Multiply.Machine.Execute(ctx);

            Assert.AreEqual(2 * 5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void MultiplyDoesNotOverflow()
        {
            var ctx = new Context();
            ctx.Stack.Push(long.MaxValue);
            ctx.Stack.Push(2);

            Assert.ThrowsException<OverflowException>(() => Multiply.Machine.Execute(ctx));
        }


        [TestMethod]
        public void Subtracts()
        {
            var ctx = new Context();
            ctx.Stack.Push(5);
            ctx.Stack.Push(2);

            Subtract.Machine.Execute(ctx);

            Assert.AreEqual(2 - 5, ctx.Stack.Pop());
        }

        [TestMethod]
        public void SubtractDoesNotOverflow()
        {
            var ctx = new Context();
            ctx.Stack.Push(long.MinValue);
            ctx.Stack.Push(long.MaxValue);

            Assert.ThrowsException<OverflowException>(() => Subtract.Machine.Execute(ctx));
        }
    }
}
