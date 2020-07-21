namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Call : IVirtualMachine
    {
        private Scope Load(byte[] id)
        {
            throw new NotImplementedException();
        }

        public void Execute(Context context)
        {
            var scopeid = context.Pop();
            var word = context.Pop();

            // TODO: do we need to push current scope id as 'from'?

            var scope = Load(scopeid.ToArray());

            context.Push(scope.Describe(word));
        }
    }
}
