using ForkingVirtualMachine.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class ArithmeticMachineTests
    {
        public static Context CreateContext()
        {
            var machine = new VirtualMachine();
            var context = new Context(machine, new byte[] {
                Reg.a, Reg.b, Reg.c, Reg.d
            });
            return context;
        }

        [TestMethod]
        public void Absolutes()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, -2);

            Abs.Machine.Execute(ctx);

            Assert.AreEqual(2, ctx.Machine.LoadInt(Reg.b));
        }

        [TestMethod]
        public void Adds()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, 5);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + 5, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void AddsNegatives()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, -5);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + -5, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void AndsTrueIfBoth1()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 1);
            ctx.Machine.Store(Reg.b, 1);

            And.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void AndsTrueIfBothNon0()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 10);
            ctx.Machine.Store(Reg.b, 111);

            And.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void AndsFalseIfAny0()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 10);
            ctx.Machine.Store(Reg.b, 0);

            And.Machine.Execute(ctx);

            Assert.AreEqual(0, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void Divides()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, 5);

            Divide.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void DividesWithRemainder()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, 5);

            DivideRem.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.Machine.LoadInt(Reg.c));
            Assert.AreEqual(2 % 5, ctx.Machine.LoadInt(Reg.d));
        }

        [TestMethod]
        public void EqualsTo()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 10);
            ctx.Machine.Store(Reg.b, 10);

            EqualTo.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void EqualsToFalse()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 10);
            ctx.Machine.Store(Reg.b, 11);

            EqualTo.Machine.Execute(ctx);

            Assert.AreEqual(0, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void GreaterThans()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 11);
            ctx.Machine.Store(Reg.b, 10);

            GreaterThan.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void GreaterThansFalse()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 11);
            ctx.Machine.Store(Reg.b, 11);

            GreaterThan.Machine.Execute(ctx);

            Assert.AreEqual(0, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void GreaterThanEquals()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 11);
            ctx.Machine.Store(Reg.b, 11);

            GreaterThanEqualTo.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void LessThans()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 10);
            ctx.Machine.Store(Reg.b, 11);

            LessThan.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void LessThansFalse()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 11);
            ctx.Machine.Store(Reg.b, 11);

            LessThan.Machine.Execute(ctx);

            Assert.AreEqual(0, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void LessThanEquals()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 11);
            ctx.Machine.Store(Reg.b, 11);

            LessThanEqualTo.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void Modulos()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, 5);

            Modulo.Machine.Execute(ctx);

            Assert.AreEqual(2 % 5, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void Multiplies()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, 5);

            Multiply.Machine.Execute(ctx);

            Assert.AreEqual(2 * 5, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void NegatesPos()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 100);

            Negate.Machine.Execute(ctx);

            Assert.AreEqual(-100, ctx.Machine.LoadInt(Reg.b));
        }

        [TestMethod]
        public void NegatesNegative()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, -100);

            Negate.Machine.Execute(ctx);

            Assert.AreEqual(100, ctx.Machine.LoadInt(Reg.b));
        }

        [TestMethod]
        public void NotsToFalse()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 100);

            Not.Machine.Execute(ctx);

            Assert.AreEqual(0, ctx.Machine.LoadInt(Reg.b));
        }

        [TestMethod]
        public void NotsFalseToTrue()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 0);

            Not.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.b));
        }

        [TestMethod]
        public void Ors()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 0);
            ctx.Machine.Store(Reg.b, 100);

            Or.Machine.Execute(ctx);

            Assert.AreEqual(1, ctx.Machine.LoadInt(Reg.c));
        }

        [TestMethod]
        public void OrsFalse()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 0);
            ctx.Machine.Store(Reg.b, 0);

            Or.Machine.Execute(ctx);

            Assert.AreEqual(0, ctx.Machine.LoadInt(Reg.c));
        }


        [TestMethod]
        public void Subtracts()
        {
            var ctx = CreateContext();
            ctx.Machine.Store(Reg.a, 2);
            ctx.Machine.Store(Reg.b, 5);

            Subtract.Machine.Execute(ctx);

            Assert.AreEqual(2 - 5, ctx.Machine.LoadInt(Reg.c));
        }
    }
}
