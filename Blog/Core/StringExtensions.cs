using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Core
{
    public static class StringExtensions
    {
        public static string ToStringList<T>(this IEnumerable<T> items)
        {
            if (items == null)
                return "(null)";

            if (!items.Any())
                return "(empty)";

            return "[" + items.Select(i => i.ToString()).Aggregate((n, p) => n + ", " + p) + "]";
        }

        public static string FullName(this Enum value)
        {
            return
                string.Format("{0}.{1}", value.GetType().FullName, value.ToString());
        }
    }
}
