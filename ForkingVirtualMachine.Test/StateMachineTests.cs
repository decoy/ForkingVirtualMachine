using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ForkingVirtualMachine.Extensions;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class StateMachineTests
    {
        [TestMethod]
        public void Defines()
        {

            var ctx = new Context(new VirtualMachine(), new byte[]
            {
                Reg.a, 2, 9, 10,    // define as 9, 10
                Reg.x,              // random op
                Reg.a, 0,           // undefine
                Reg.y,              // random op
                Reg.a, 0,           // undefine
                0, 1, 100,          // try to define 0
                1, 1, 100,          // try to define 1
            });

            Define.Machine.Execute(ctx);

            // the word is the correct length and has values
            var data = ctx.Machine.Load(Reg.a);
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual(9, data.Span[0]);
            Assert.AreEqual(10, data.Span[1]);

            // the execution has been forwarded
            Assert.AreEqual(Reg.x, ctx.Next());

            // undefines
            Define.Machine.Execute(ctx);
            data = ctx.Machine.Load(Reg.a);
            Assert.AreEqual(0, data.Length);
            Assert.AreEqual(Reg.y, ctx.Next());

            // current behavior is no error on undefining
            Define.Machine.Execute(ctx);
            data = ctx.Machine.Load(Reg.a);
            Assert.AreEqual(0, data.Length);

            // eats the 0
            Define.Machine.Execute(ctx);
            data = ctx.Machine.Load(0);
            Assert.AreEqual(0, data.Length);

            Assert.ThrowsException<BoundaryException>(() => Define.Machine.Execute(ctx));

            Assert.IsTrue(ctx.IsComplete);
        }

        [TestMethod]
        public void LimitsStore()
        {
            var data = new byte[VirtualMachine.MAX_REGISTER_SIZE];
            var data2 = new byte[VirtualMachine.MAX_REGISTER_SIZE + 1];

            var vm = new VirtualMachine();
            vm.Store(Reg.a, data);

            var a = vm.Load(Reg.a);
            Assert.AreEqual(VirtualMachine.MAX_REGISTER_SIZE, a.Length);

            Assert.ThrowsException<BoundaryException>(() =>
            {
                vm.Store(Reg.b, data2);
            });
        }
    }
}