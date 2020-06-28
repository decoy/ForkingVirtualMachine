using ForkingVirtualMachine.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class ArithmeticMachineTests
    {
        public static Context CreateContext(byte[] data)
        {
            var machine = new VirtualMachine();
            var context = new Context(machine, data);
            return context;
        }

        [TestMethod]
        public void Adds()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, 5);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + 5, ctx.Machine.LoadInt(3));
        }

        [TestMethod]
        public void AddsNegatives()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, -5);

            Add.Machine.Execute(ctx);

            Assert.AreEqual(2 + -5, ctx.Machine.LoadInt(3));
        }

        [TestMethod]
        public void Divides()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, 5);

            Divide.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.Machine.LoadInt(3));
        }

        [TestMethod]
        public void DividesWithRemainder()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3, 4
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, 5);

            DivideRem.Machine.Execute(ctx);

            Assert.AreEqual(2 / 5, ctx.Machine.LoadInt(3));
            Assert.AreEqual(2 % 5, ctx.Machine.LoadInt(4));
        }

        [TestMethod]
        public void Modulos()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3, 4
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, 5);

            Modulo.Machine.Execute(ctx);

            Assert.AreEqual(2 % 5, ctx.Machine.LoadInt(3));
        }

        [TestMethod]
        public void Multiplies()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3, 4
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, 5);

            Multiply.Machine.Execute(ctx);

            Assert.AreEqual(2 * 5, ctx.Machine.LoadInt(3));
        }

        [TestMethod]
        public void Subtracts()
        {
            var ctx = CreateContext(new byte[] {
                1, 2, 3, 4
            });
            ctx.Machine.Store(1, 2);
            ctx.Machine.Store(2, 5);

            Subtract.Machine.Execute(ctx);

            Assert.AreEqual(2 - 5, ctx.Machine.LoadInt(3));
        }
    }
}
