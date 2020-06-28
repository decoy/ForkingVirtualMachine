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
