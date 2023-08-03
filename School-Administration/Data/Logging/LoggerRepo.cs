using School_Administration.Data.Logs;
using System.Collections.Generic;

namespace School_Administration.Data.Logging
{
    public class LoggerRepo:ILoggerRepo
    {
        public void AddToLogs(Log log)
        {
            LoggerStore.Logs.Add(log);
        }

        public List<Log> GetAllLogs()
        {
            return LoggerStore.Logs;
        }
    }
}
