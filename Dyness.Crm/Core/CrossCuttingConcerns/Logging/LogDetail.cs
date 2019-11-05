using System;
using System.Collections.Generic;

namespace Core.CrossCuttingConcerns.Logging
{
    public class LogDetail
    {
        public string FullName { get; set; }

        public string EntityName { get; set; }

        public string MethodName { get; set; }

        public string Date { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public List<LogParameter> Parameters { get; set; }

        public Exception Exception { get; set; }
    }
}
