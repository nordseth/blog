using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Web.App.Start
{
    public  class Log4NetAdapter : Blog.Core.ILog
    {
        private log4net.ILog _log;
        public Log4NetAdapter(log4net.ILog log)
        {
            _log = log;
        }

        public void Info(object message, Exception e = null)
        {
            _log.Info(message, e);
        }

        public void InfoFormat(string pattern, params object[] args)
        {
            _log.InfoFormat(pattern, args);
        }

        public void Debug(object message, Exception e = null)
        {
            _log.Debug(message, e);
        }

        public void DebugFormat(string pattern, params object[] args)
        {
            _log.DebugFormat(pattern, args);
        }

        public void Error(object message, Exception e = null)
        {
            _log.Error(message, e);
        }

        public void ErrorFormat(string pattern, params object[] args)
        {
            _log.ErrorFormat(pattern, args);
        }
    }
}
