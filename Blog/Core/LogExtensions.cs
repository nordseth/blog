using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Core
{
    public static class LogExtensions
    {
        public static ILog Log(this object o)
        {
            if (o == null)
                throw new ArgumentNullException();
            return LogManager.GetLogger(o.GetType());
        }
    }
}
