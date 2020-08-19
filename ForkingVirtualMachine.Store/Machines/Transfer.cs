namespace ForkingVirtualMachine.Store.Machines
{
    using ForkingVirtualMachine.Store.Database;
    using System.Threading.Tasks;

    public class Transfer : IAsyncVirtualMachine
    {
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
            // context, from, to, delta
            var from = scope.Id;

            // relative or id?
            // is the path the id?
            // from _is_ a context...
            // hrmmmmmmmm let's go with an id for now.

            var ctxId = context.Pop().ToArray(); // /g1/green/p1
            var toId = context.Pop().ToArray(); // /g1/a1/u2


            var node = await db.GetNode(ctxId);


            // get node by context + from
            // get a 'to' node

            // do this in the db?
            // fromnode.value--;
            // tonode.value++;

            // load 'to' scope

            // if !tonode.onreceive() rollback();            
        }
    }
}
