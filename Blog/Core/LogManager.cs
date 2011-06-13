using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Core
{
    public static class LogManager
    {
        private static Func<Type, ILog> s_factoryMethod;
        private static NullLog s_nullLog = new NullLog();
        public static void SetFactoryMethod(Func<Type, ILog> factoryMethod)
        {
            s_factoryMethod = factoryMethod;
            factoryMethod(typeof(LogManager)).Debug("Factory method set");
        }

        public static ILog GetLogger(Type type)
        {
            if (s_factoryMethod == null)
                return s_nullLog;
            else
                return s_factoryMethod(type);
        }

        private class NullLog : ILog
        {
            public void Info(object message, Exception e = null) { }
            public void InfoFormat(string pattern, params object[] args) { }
            public void Debug(object message, Exception e = null) { }
            public void DebugFormat(string pattern, params object[] args) { }
            public void Error(object message, Exception e = null) { }
            public void ErrorFormat(string pattern, params object[] args) { }
        }
    }

}
