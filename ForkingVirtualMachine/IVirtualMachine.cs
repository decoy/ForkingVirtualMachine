namespace ForkingVirtualMachine
{
    public interface IVirtualMachine
    {
        public void Execute(IScope scope, IContext context);
    }
}
