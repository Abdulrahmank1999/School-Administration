using System;

namespace School_Administration.Data.Logs
{
    public class Log
    {
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Method { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public string ResponseCode { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime RespondedOn { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}
