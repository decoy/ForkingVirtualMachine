using ForkingVirtualMachine.Machines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class ArithmeticMachineTests
    {
        [TestMethod]
        public void Absolutes()
        {
            var ctx = new Context();
            ctx.Push(-5);

            Abs.Machine.Execute(ctx);

            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void Adds()
        {
            var ctx = new Context();
            ctx.Push(2);
            ctx.Push(5);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + 5, ctx.PopInt());
        }

        [TestMethod]
        public void AddsNegatives()
        {
            var ctx = new Context();
            ctx.Push(2);
            ctx.Push(-5);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + -5, ctx.PopInt());
        }

        [TestMethod]
        public void AndsTrueIfBoth1()
        {
            var ctx = new Context();
            ctx.Push(Constants.True);
            ctx.Push(Constants.True);

            And.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void AndsTrueIfBothNon0()
        {
            var ctx = new Context();
            ctx.Push(100);
            ctx.Push(200);

            And.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void AndsFalseIfAny0()
        {
            var ctx = new Context();
            ctx.Push(Constants.False);
            ctx.Push(Constants.True);

            And.Machine.Execute(ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void Divides()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            Divide.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.PopInt());
        }

        [TestMethod]
        public void DividesWithRemainder()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            DivideRem.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.PopInt());
            Assert.AreEqual(2 % 5, ctx.PopInt());
        }

        [TestMethod]
        public void EqualsTo()
        {
            var ctx = new Context();
            ctx.Push(10);
            ctx.Push(10);

            EqualTo.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void EqualsToFalse()
        {
            var ctx = new Context();
            ctx.Push(10);
            ctx.Push(11);

            EqualTo.Machine.Execute(ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void GreaterThans()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(7);

            GreaterThan.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void GreaterThansFalse()
        {
            var ctx = new Context();
            ctx.Push(7);
            ctx.Push(7);

            GreaterThan.Machine.Execute(ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void GreaterThanEquals()
        {
            var ctx = new Context();
            ctx.Push(7);
            ctx.Push(7);

            GreaterThanEqualTo.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void IfsTrue()
        {
            var ctx = new Context();
            ctx.Push(100);
            ctx.Push(200);
            ctx.Push(1);

            If.Machine.Execute(ctx);

            Assert.AreEqual(200, ctx.PopInt());
            Assert.AreEqual(100, ctx.PopInt());
        }

        [TestMethod]
        public void IfsFalse()
        {
            var ctx = new Context();
            ctx.Push(100);
            ctx.Push(200);
            ctx.Push(0);

            If.Machine.Execute(ctx);

            Assert.AreEqual(100, ctx.PopInt());
        }

        [TestMethod]
        public void LessThans()
        {
            var ctx = new Context();
            ctx.Push(7);
            ctx.Push(6);

            LessThan.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void LessThansFalse()
        {
            var ctx = new Context();
            ctx.Push(7);
            ctx.Push(7);

            LessThan.Machine.Execute(ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void LessThanEquals()
        {
            var ctx = new Context();
            ctx.Push(7);
            ctx.Push(7);

            LessThanEqualTo.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void Maxes()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            Max.Machine.Execute(ctx);

            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void Mins()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            Min.Machine.Execute(ctx);

            Assert.AreEqual(2, ctx.PopInt());
        }

        [TestMethod]
        public void Modulos()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            Modulo.Machine.Execute(ctx);

            Assert.AreEqual(2 % 5, ctx.PopInt());
        }

        [TestMethod]
        public void Multiplies()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            Multiply.Machine.Execute(ctx);

            Assert.AreEqual(2 * 5, ctx.PopInt());
        }

        [TestMethod]
        public void NegatesPos()
        {
            var ctx = new Context();
            ctx.Push(5);

            Negate.Machine.Execute(ctx);

            Assert.AreEqual(-5, ctx.PopInt());
        }

        [TestMethod]
        public void NegatesNegative()
        {
            var ctx = new Context();
            ctx.Push(-5);

            Negate.Machine.Execute(ctx);

            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void NotsToFalse()
        {
            var ctx = new Context();
            ctx.Push(100);

            Not.Machine.Execute(ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void NotsFalseToTrue()
        {
            var ctx = new Context();
            ctx.Push(0);

            Not.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void Ors()
        {
            var ctx = new Context();
            ctx.Push(0);
            ctx.Push(1);

            Or.Machine.Execute(ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void OrsFalse()
        {
            var ctx = new Context();
            ctx.Push(0);
            ctx.Push(0);

            Or.Machine.Execute(ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void Subtracts()
        {
            var ctx = new Context();
            ctx.Push(5);
            ctx.Push(2);

            Subtract.Machine.Execute(ctx);

            Assert.AreEqual(2 - 5, ctx.PopInt());
        }
    }
}
