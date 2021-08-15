using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DebugLogger.Wpf
{
    public class LogData
    {
        public DefaultLogType LogType { get; private set; }

        public string log { get; private set; }

        public DateTime initiatedTime { get; private set; }

        public string timePeriod { get { return $"[{initiatedTime.ToString("hh:mm:ss:ff")}]"; } }

        public LogData(DefaultLogType type, string log)
        {
            initiatedTime = DateTime.Now;

            LogType = type;
            this.log = log;
        }
    }

    public enum DefaultLogType
    {
        Master,
        Warning,
        Error
    }
}
