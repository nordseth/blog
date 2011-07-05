using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Concepts
{
    public class DenormalizedReference<T> where T : INamedDocument
    {
        public int Id { get; set; }

        public static implicit operator DenormalizedReference<T>(T doc)
        {
            return new DenormalizedReference<T>
            {
                Id = doc.Id
            };
        }
    }
}
