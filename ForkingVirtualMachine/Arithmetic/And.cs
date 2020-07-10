namespace ForkingVirtualMachine.Arithmetic
{
    public class And : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new And();

        public void Execute(Context context)
        {
            var a = context.PopBool();
            var b = context.PopBool();
            context.Push((a && b) 
                ? Constants.True 
                : Constants.False);
        }
    }
}
