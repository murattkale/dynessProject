using log4net;
using System;

namespace Core.CrossCuttingConcerns.Logging
{
    [Serializable]
    public class LoggerService
    {
        private ILog log;

        public LoggerService(ILog log)
        {
            this.log = log;
        }

        public bool IsInfoEnabled => log.IsInfoEnabled;

        public bool IsDebugEnabled => log.IsDebugEnabled;

        public bool IsFatalEnabled => log.IsFatalEnabled;

        public bool IsErrorEnabled => log.IsErrorEnabled;

        public bool IsWarnEnabled => log.IsWarnEnabled;

        public void Info(object message)
        {
            if (IsInfoEnabled)
                log.Info(message);
        }

        public void Debug(object message)
        {
            if (IsDebugEnabled)
                log.Debug(message);
        }

        public void Fatal(object message)
        {
            if (IsFatalEnabled)
                log.Fatal(message);
        }

        public void Error(object message)
        {
            if (IsErrorEnabled)
                log.Error(message);
        }

        public void Warn(object message)
        {
            if (IsWarnEnabled)
                log.Warn(message);
        }
    }
}
