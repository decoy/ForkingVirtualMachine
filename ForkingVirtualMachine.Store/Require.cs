//using System;

//namespace ForkingVirtualMachine.Store
//{
//    public class Require : IVirtualMachine
//    {
//        private Manager manager;
//        private VirtualMachine machine;

//        public Require(VirtualMachine machine, Manager manager)
//        {
//            this.machine = machine;
//            this.manager = manager;
//        }

//        public void Execute(Context context)
//        {
//            var word = context.Pop().Span[0];
//            var id = context.Pop();

//            if (word == 0)
//            {
//                return;
//            }

//            var exe = manager.Load(id.ToArray());
//            if (exe.Machine == null)
//            {
//                exe = new Executable(machine, exe.Data, exe.Data);
//            }
//            throw new NotImplementedException();
//            //machine.Set(word, exe);
//        }
//    }
//}
