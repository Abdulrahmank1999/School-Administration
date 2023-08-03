using School_Administration.Data.Logs;
using System.Collections.Generic;

namespace School_Administration.Data.Logging
{
    public interface ILoggerRepo
    {
        void AddToLogs(Log log);

        List<Log> GetAllLogs();
    }
}
