using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Concepts
{
    public class Product : INamedDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public PriceType PriceType { get; set; }
    }
}
