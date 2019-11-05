using log4net;

namespace Core.CrossCuttingConcerns.Logging.Log4net.Loggers
{
    public class FileLogger : LoggerService
    {
        public FileLogger() : base(LogManager.GetLogger("JsonFileLogger"))
        {
        }
    }
}
