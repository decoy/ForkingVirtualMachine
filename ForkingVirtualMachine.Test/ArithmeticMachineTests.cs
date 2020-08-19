using ForkingVirtualMachine.Machines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class ArithmeticMachineTests
    {
        public static Context Create()
        {
            return new Context();
        }

        [TestMethod]
        public void Absolutes()
        {
            var ctx = Create();
            ctx.Push(-5);

            Abs.Machine.Execute(null, ctx);

            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void Adds()
        {
            var ctx = Create();
            ctx.Push(2);
            ctx.Push(5);

            Add.Machine.Execute(null, ctx);

            Assert.AreEqual(2 + 5, ctx.PopInt());
        }

        [TestMethod]
        public void AddsNegatives()
        {
            var ctx = Create();
            ctx.Push(2);
            ctx.Push(-5);

            Add.Machine.Execute(null, ctx);

            Assert.AreEqual(2 + -5, ctx.PopInt());
        }

        [TestMethod]
        public void AndsTrueIfBoth1()
        {
            var ctx = Create();
            ctx.Push(Constants.True);
            ctx.Push(Constants.True);

            And.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void AndsTrueIfBothNon0()
        {
            var ctx = Create();
            ctx.Push(100);
            ctx.Push(200);

            And.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void AndsFalseIfAny0()
        {
            var ctx = Create();
            ctx.Push(Constants.False);
            ctx.Push(Constants.True);

            And.Machine.Execute(null, ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void Divides()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            Divide.Machine.Execute(null, ctx);

            Assert.AreEqual(2 / 5, ctx.PopInt());
        }

        [TestMethod]
        public void DividesWithRemainder()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            DivideRem.Machine.Execute(null, ctx);

            Assert.AreEqual(2 / 5, ctx.PopInt());
            Assert.AreEqual(2 % 5, ctx.PopInt());
        }

        [TestMethod]
        public void EqualsTo()
        {
            var ctx = Create();
            ctx.Push(10);
            ctx.Push(10);

            EqualTo.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void EqualsToFalse()
        {
            var ctx = Create();
            ctx.Push(10);
            ctx.Push(11);

            EqualTo.Machine.Execute(null, ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void GreaterThans()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(7);

            GreaterThan.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void GreaterThansFalse()
        {
            var ctx = Create();
            ctx.Push(7);
            ctx.Push(7);

            GreaterThan.Machine.Execute(null, ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void GreaterThanEquals()
        {
            var ctx = Create();
            ctx.Push(7);
            ctx.Push(7);

            GreaterThanEqualTo.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void IfThensTrue()
        {
            var ctx = Create();
            ctx.Push(100);
            ctx.Push(1);
            ctx.Push(200);

            IfThen.Machine.Execute(null, ctx);

            Assert.AreEqual(200, ctx.PopInt());
            Assert.AreEqual(100, ctx.PopInt());
        }

        [TestMethod]
        public void IfThensFalse()
        {
            var ctx = Create();
            ctx.Push(100);
            ctx.Push(0);
            ctx.Push(200);

            IfThen.Machine.Execute(null, ctx);

            Assert.AreEqual(100, ctx.PopInt());
        }

        [TestMethod]
        public void IfThenElsesTrue()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(1);
            ctx.Push(100);
            ctx.Push(200);

            IfThenElse.Machine.Execute(null, ctx);

            Assert.AreEqual(200, ctx.PopInt());
            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void IfThenElsesFalse()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(0);
            ctx.Push(100);
            ctx.Push(200);

            IfThenElse.Machine.Execute(null, ctx);

            Assert.AreEqual(100, ctx.PopInt());
            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void LessThans()
        {
            var ctx = Create();
            ctx.Push(7);
            ctx.Push(6);

            LessThan.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void LessThansFalse()
        {
            var ctx = Create();
            ctx.Push(7);
            ctx.Push(7);

            LessThan.Machine.Execute(null, ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void LessThanEquals()
        {
            var ctx = Create();
            ctx.Push(7);
            ctx.Push(7);

            LessThanEqualTo.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void Maxes()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            Max.Machine.Execute(null, ctx);

            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void Mins()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            Min.Machine.Execute(null, ctx);

            Assert.AreEqual(2, ctx.PopInt());
        }

        [TestMethod]
        public void Modulos()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            Modulo.Machine.Execute(null, ctx);

            Assert.AreEqual(2 % 5, ctx.PopInt());
        }

        [TestMethod]
        public void Multiplies()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            Multiply.Machine.Execute(null, ctx);

            Assert.AreEqual(2 * 5, ctx.PopInt());
        }

        [TestMethod]
        public void NegatesPos()
        {
            var ctx = Create();
            ctx.Push(5);

            Negate.Machine.Execute(null, ctx);

            Assert.AreEqual(-5, ctx.PopInt());
        }

        [TestMethod]
        public void NegatesNegative()
        {
            var ctx = Create();
            ctx.Push(-5);

            Negate.Machine.Execute(null, ctx);

            Assert.AreEqual(5, ctx.PopInt());
        }

        [TestMethod]
        public void NotsToFalse()
        {
            var ctx = Create();
            ctx.Push(100);

            Not.Machine.Execute(null, ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void NotsFalseToTrue()
        {
            var ctx = Create();
            ctx.Push(0);

            Not.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void Ors()
        {
            var ctx = Create();
            ctx.Push(0);
            ctx.Push(1);

            Or.Machine.Execute(null, ctx);

            Assert.AreEqual(true, ctx.PopBool());
        }

        [TestMethod]
        public void OrsFalse()
        {
            var ctx = Create();
            ctx.Push(0);
            ctx.Push(0);

            Or.Machine.Execute(null, ctx);

            Assert.AreEqual(false, ctx.PopBool());
        }

        [TestMethod]
        public void Subtracts()
        {
            var ctx = Create();
            ctx.Push(5);
            ctx.Push(2);

            Subtract.Machine.Execute(null, ctx);

            Assert.AreEqual(2 - 5, ctx.PopInt());
        }
    }
}
