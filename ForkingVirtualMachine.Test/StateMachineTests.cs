using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForkingVirtualMachine.Test
{
    [TestClass]
    public class StateMachineTests
    {
        [TestMethod]
        public void Defines()
        {
            byte word = 240;
            var ctx = new Context(new VirtualMachine(), new byte[]
            {
                word, 2, 9, 10, // define as 9, 10
                250,            // random op
                word, 0,        // undefine
                251,            // random op
                word, 0,        // undefine
                0, 1, 100,      // try to define 0
            });

            Define.Machine.Execute(ctx);

            // the word is the correct length and has values
            var data = ctx.Machine.Load(word);
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual(9, data.Span[0]);
            Assert.AreEqual(10, data.Span[1]);

            // the execution has been forwarded
            Assert.AreEqual(250, ctx.Next());

            // undefines
            Define.Machine.Execute(ctx);
            data = ctx.Machine.Load(word);
            Assert.AreEqual(0, data.Length);
            Assert.AreEqual(251, ctx.Next());

            // current behavior is no error on undefining
            Define.Machine.Execute(ctx);
            data = ctx.Machine.Load(word);
            Assert.AreEqual(0, data.Length);


            // eats the 0
            Define.Machine.Execute(ctx);
            data = ctx.Machine.Load(0);
            Assert.AreEqual(0, data.Length);

            Assert.IsTrue(ctx.IsComplete);
        }
    }
}