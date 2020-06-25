namespace ForkingVirtualMachine.State
{
    public class PushN : IVirtualMachine
    {
        public void Execute(Context context)
        {
            var n = context.Execution.Next();
            for (var i = 0; i < n; i++)
            {
                context.Stack.Push(context.Execution.Next());
            }
        }
    }
}
