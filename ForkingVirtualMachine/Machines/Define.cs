namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Define : IScope
    {
        public static readonly byte[] UID = Guid.Parse("CAFA8CAD-0807-4984-95F2-9C7B8B751BCA").ToByteArray();

        public static readonly IScope Machine = new Define();

        public byte[] Id => UID;

        public void Execute(IContext context)
        {
            var word = context.Pop().ToArray();
            var data = context.Pop();

            context.Define(word, data);
        }
    }
}
