namespace ForkingVirtualMachine.Store.Models
{
    using System;

    public class Migration
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime ran_on { get; set; }
    }
}
