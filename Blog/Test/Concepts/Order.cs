using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Concepts
{
    public class Order : INamedDocument
    {
        public int Id { get; set; }
        public DenormalizedReference<Customer> Customer { get; set; }
        public List<Contract> Items { get; set; }
        public Address ShippingAddress { get; set; }
    }
}
