using ForkingVirtualMachine.Extensions;
using ForkingVirtualMachine.Arithmetic;
using ForkingVirtualMachine.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ForkingVirtualMachine.Flow;

namespace ForkingVirtualMachine.Test
{
    //[TestClass]
    //public class VirtualMachineTests
    //{
    //    public static class Op
    //    {
    //        public const byte No = 0;
    //        public const byte Push = 1;
    //        public const byte PushN = 2;
    //        public const byte Define = 3;
    //        public const byte Add = 20;
    //        public const byte Print = 100;
    //        public const byte Dupe = 22;
    //    }

    //    public class Collector : IVirtualMachine
    //    {
    //        public Queue<long> Collected { get; } = new Queue<long>();

    //        public void Execute(Context context)
    //        {
    //            Collected.Enqueue(context.Stack.Pop());
    //        }
    //    }

    //    private static VirtualMachine CreateTestVm(Collector col)
    //    {
    //        return new VirtualMachine()
    //            .Add(Op.No, SafeWord.Machine)
    //            .Add(Op.Push, Push.Machine)
    //            .Add(Op.Add, Add.Machine)
    //            .Add(Op.Define, Define.Machine)
    //            .Add(Op.PushN, PushN.Machine)
    //            .Add(Op.Dupe, Dupe.Machine)
    //            .Add(Op.Print, col);
    //    }

    //    [TestMethod]
    //    public void MoreTesting()
    //    {
    //        var col = new Collector();
    //        var trash = CreateTestVm(col);
    //        var router = new Router(trash.Machines);
    //        var ctx = trash.Fork();
    //        var vm = new Operation(router);

    //        byte word = 240;

    //        var program = new List<byte>()
    //            // create a function (adds 100)
    //            .AddProgram(Op.Define, word)
    //            .AddData(Op.Push, 100, Op.Add)

    //            // Push 5 and 2 to the stack
    //            .AddProgram(Op.PushN)
    //            .AddData(5, 2)

    //            // add 5 + 2, dupe, print
    //            .AddProgram(Op.Add, Op.Dupe, Op.Print)

    //            // then run the program +100 funcion and print
    //            .AddProgram(word, Op.Print)
    //            .ToExecution(ctx);


    //        ctx.Executions.Push(program);

    //        while (ctx.Executions.Count > 0)
    //        {
    //            vm.Execute(ctx);
    //        }

    //        var x = col;
    //    }

    //    [TestMethod]
    //    public void RunsMachine()
    //    {
    //        var col = new Collector();
    //        var vm = CreateTestVm(col);
    //        var ctx = vm.Fork();

    //        var program = new List<byte>()
    //            .AddProgram(Op.Push, 5, Op.Push, 2, Op.Add, Op.Print)
    //            .ToExecution(ctx);


    //        ctx.Executions.Push(program);

    //        vm.Run(ctx);

    //        Assert.AreEqual(7, col.Collected.Dequeue());
    //    }

    //    [TestMethod]
    //    public void TestyTestums2()
    //    {
    //        var col = new Collector();
    //        var trash = CreateTestVm(col);
    //        var router = new Router(trash.Machines);
    //        var ctx = trash.Fork();
    //        var vm = new Operation(router);

    //        byte word = 240;

    //        var program = new List<byte>()
    //            // create a function (adds 100)
    //            .AddProgram(Op.Define, word)
    //            .AddData(Op.Push, 100, Op.Add)

    //            // Push 5 and 2 to the stack
    //            .AddProgram(Op.PushN)
    //            .AddData(5, 2)

    //            // add 5 + 2, dupe, print
    //            .AddProgram(Op.Add, Op.Dupe, Op.Print)

    //            // then run the program +100 funcion and print
    //            .AddProgram(word, Op.Print)
    //            .ToExecution(ctx);


    //        ctx.Executions.Push(program);

    //        while (ctx.Executions.Count > 0)
    //            vm.Execute(ctx);

    //        Assert.AreEqual(5 + 2, col.Collected.Dequeue());
    //        Assert.AreEqual(5 + 2 + 100, col.Collected.Dequeue());

    //        var ctx2 = ctx.Fork(word, Op.Print, Op.Push);
    //        var program2 = new List<byte>()
    //            .AddProgram(Op.Push, 99, word, Op.Print)
    //            .ToExecution(ctx2);

    //        ctx2.Executions.Push(program2);
    //        while (ctx.Executions.Count > 0)
    //            vm.Execute(ctx2);

    //        Assert.AreEqual(99 + 100, col.Collected.Dequeue());
    //    }

    //    [TestMethod]
    //    public void TestyTestums()
    //    {
    //        var col = new Collector();
    //        var vm = CreateTestVm(col);
    //        var ctx = vm.Fork();

    //        byte word = 240;

    //        var program = new List<byte>()
    //            // create a function (adds 100)
    //            .AddProgram(Op.Define, word)
    //            .AddData(Op.Push, 100, Op.Add)

    //            // Push 5 and 2 to the stack
    //            .AddProgram(Op.PushN)
    //            .AddData(5, 2)

    //            // add 5 + 2, dupe, print
    //            .AddProgram(Op.Add, Op.Dupe, Op.Print)

    //            // then run the program +100 funcion and print
    //            .AddProgram(word, Op.Print)
    //            .ToExecution(ctx);


    //        ctx.Executions.Push(program);

    //        vm.Run(ctx);

    //        Assert.AreEqual(5 + 2, col.Collected.Dequeue());
    //        Assert.AreEqual(5 + 2 + 100, col.Collected.Dequeue());

    //        var ctx2 = ctx.Fork(word, Op.Print, Op.Push);
    //        var program2 = new List<byte>()
    //            .AddProgram(Op.Push, 99, word, Op.Print)
    //            .ToExecution(ctx2);

    //        ctx2.Executions.Push(program2);
    //        vm.Run(ctx2);

    //        Assert.AreEqual(99 + 100, col.Collected.Dequeue());
    //    }
    //}
}
