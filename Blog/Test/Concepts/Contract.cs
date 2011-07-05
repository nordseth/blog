using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Concepts
{
    public class Contract
    {
        public DenormalizedReference<Product> Product { get; set; }
        public int Count { get; set; }
        public double? Price { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
