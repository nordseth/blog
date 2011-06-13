using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Core
{
    public  interface ILog
    {
        void Info(object message, Exception e = null);
        void InfoFormat(string pattern, params object[] args);
        void Debug(object message, Exception e = null);
        void DebugFormat(string pattern, params object[] args);
        void Error(object message, Exception e = null);
        void ErrorFormat(string pattern, params object[] args);
    }
}
