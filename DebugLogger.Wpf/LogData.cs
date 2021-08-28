using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DebugLogger.Wpf
{
    public class LogData
    {
        public Enum logType { get; private set; }
        public string logTypeS => logType.ToString(); // Log Type String

        public string log { get; private set; }
        public int logHash => log.GetHashCode();

        public List<DateTime> occurrenceTime { get; private set; }
        public DateTime latestOccurrence => occurrenceTime[occurrenceTime.Count - 1];

        public int logCount => occurrenceTime.Count;

        public LogData(DefaultLogType type, string log)
        {
            occurrenceTime = new List<DateTime> { DateTime.Now };

            logType = type;
            this.log = log;
        }
        
        #region Extension
        public static bool operator ==(LogData a, LogData b)
        {
            return a.logHash == b.logHash;
        }
        public static bool operator !=(LogData a, LogData b)
        {
            return a.logHash != b.logHash;
        }

        public static LogData operator +(LogData a, LogData b)
        {
            a.occurrenceTime = a.occurrenceTime.Concat(b.occurrenceTime).ToList();

            return a;
        }

        public override bool Equals(object o)
        {
            var b = (LogData)o;
            return logHash == b.logHash;
        }

        public override int GetHashCode()  //??????????
        {
            return -1;
        }
        #endregion      
    }

    public static class LogDataExtension
    {
        public static string defaultFormat(this DateTime dt)
        {
            return $"[{dt.ToString("hh:mm:ss:fff")}]";
        }

    }

    public enum DefaultLogType
    {
        Master,
        Warning,
        Error
    } 
}
