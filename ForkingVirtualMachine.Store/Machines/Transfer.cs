namespace ForkingVirtualMachine.Store.Machines
{
    using ForkingVirtualMachine.Store.Database;
    using System;
    using System.Threading.Tasks;

    public class Transfer : IAsyncVirtualMachine
    {
        public static readonly byte[] ON_RECEIEVE = Guid.Parse("FB5DF695-C745-4935-ACA6-B49EC9E00FD8").ToByteArray();

        public static readonly byte[] ON_RECEIVE_EXECUTE = ProgramBuilder.Create(p => p.Execute(ON_RECEIEVE));

        private readonly Repository db;

        public Transfer(Repository db)
        {
            this.db = db;
        }

        public void Execute(IScope scope, IContext context)
        {
            ExecuteAsync(scope, context).GetAwaiter().GetResult();
        }

        public async Task ExecuteAsync(IScope scope, IContext context)
        {
            var ctxId = scope.Id;
            var from = scope.Caller.Id;
            var to = context.Pop().ToArray();
            var delta = context.PopInt();

            var toNode = await db.Transfer(ctxId, from, to, delta);

            // TODO: do we allow it a caller here?
            // how to allow other games to do this?
            var toScope = await db.LoadScope(scope, toNode);

            var res = new Context();
            res.Push(delta);
            res.Push(ctxId);
            res.Push(from);

            res.Run(toScope, ON_RECEIVE_EXECUTE);

            if (!res.PopBool())
            {
                throw new Exception();
            }
        }
    }
}
