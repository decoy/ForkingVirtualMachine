namespace ForkingVirtualMachine.Extra
{
    using System;

    public class Print : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Print();

        public void Execute(Context context)
        {
            Console.WriteLine(context.Stack.Pop());
        }
    }
}
