﻿namespace ForkingVirtualMachine.Machines
{
    using System;

    public class Define : IVirtualMachine, IDescribe
    {
        private Scope scope;

        public Define(Scope scope)
        {
            this.scope = scope;
        }

        public void Execute(Context context)
        {
            var word = context.Pop();
            var data = context.Pop();

            scope = scope.Fork(null);
            scope.Set(
                word.ToArray(),
                new Executable(scope, data));
        }

        public IVirtualMachine Describe(ReadOnlyMemory<byte> word)
        {
            return scope.Describe(word);
        }
    }
}
