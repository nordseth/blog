using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Concepts
{
    public class Customer : INamedDocument
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public Person Person { get; set; }

        public List<Contract> OwnedProducts { get; set; }
    }
}
