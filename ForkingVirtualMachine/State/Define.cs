namespace ForkingVirtualMachine.State
{
    public class Define : IVirtualMachine
    {
        public static readonly IVirtualMachine Machine = new Define();

        public void Execute(Context context)
        {
            var word = context.Execution.Next();
            var ndef = context.Execution.Next();

            if (ndef < 1) // undefine
            {
                context.Functions.Remove(word);
                return;
            }

            var data = new byte[ndef + 1];
            data[0] = 0; // identifies this as a local scope function 

            for (var i = 1; i <= ndef; i++)
            {
                data[i] = context.Execution.Next();
            }

            if (context.Functions.ContainsKey(word))
            {
                context.Functions[word] = new Execution(data);
            }
            else
            {
                context.Functions.Add(word, new Execution(data));
            }
        }
    }
}
