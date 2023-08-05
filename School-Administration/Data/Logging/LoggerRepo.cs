using Microsoft.VisualBasic;
using School_Administration.Data.Logs;
using School_Administration.Extensions;
using School_Administration.Repositories.Interface;
using System;
using System.Collections.Generic;

namespace School_Administration.Data.Logging
{
    public class LoggerRepo:ILoggerRepo
    {

        public void AddToLogs(Log log)
        {
            var logs = new List<Log>
            {
                log
            };


            logs.ExportToText("Logs/" + DateTime.Now.ToString("MM-dd-yyyyTHH-mm-ss"), '|');
        }

       
    }
}
