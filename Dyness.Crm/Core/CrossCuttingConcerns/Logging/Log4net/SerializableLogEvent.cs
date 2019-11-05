using log4net.Core;
using System;

namespace Core.CrossCuttingConcerns.Logging.Log4net
{
    [Serializable]
    public class SerializableLogEvent
    {
        private LoggingEvent loggingEvent;

        public SerializableLogEvent()
        {

        }

        public SerializableLogEvent(LoggingEvent loggingEvent)
        {
            this.loggingEvent = loggingEvent;
            UserName = loggingEvent.UserName;
            MessageObject = (LogDetail)loggingEvent.MessageObject;
        }

        public string UserName { get; set; }

        public LogDetail MessageObject { get; set; }
    }
}
