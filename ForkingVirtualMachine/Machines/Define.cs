namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Define : IVirtualMachine
    {
        public static readonly byte[] UID = Guid.Parse("CAFA8CAD-0807-4984-95F2-9C7B8B751BCA").ToByteArray();

        public static readonly IVirtualMachine Machine = new Define();

        public byte[] Id => UID;

        public void Execute(IScope scope, IContext context)
        {
            var word = context.Pop().ToArray();
            var data = context.Pop();
            scope.Set(word, new Executable(data));
        }
    }
}
