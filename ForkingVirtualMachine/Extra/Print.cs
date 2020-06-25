namespace ForkingVirtualMachine.Extra
{
    using System;

    public class Print : IVirtualMachine
    {
        public void Execute(Context context)
        {
            Console.WriteLine(context.Stack.Pop());
        }
    }
}
