using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DebugLogger.Wpf
{
    public class LogData
    {
        public DefaultLogType LogType { get; private set; }
        public string LogTypeS => LogType.ToString(); // Log Type String

        public string log { get; private set; }

        public List<DateTime> occurrenceTime { get; private set; }
        public DateTime latestOccurrence => occurrenceTime[occurrenceTime.Count - 1];

        public int logCount => occurrenceTime.Count;

        public LogData(DefaultLogType type, string log)
        {
            occurrenceTime = new List<DateTime> { DateTime.Now };

            LogType = type;
            this.log = log;
        }
        /*
        #region Extension
        public static bool operator ==(LogData a, LogData b)
        {
            return a.log == b.log;
        }
        public static bool operator !=(LogData a, LogData b)
        {
            return a.log != b.log;
        }

        public static bool operator +(LogData a, LogData b)
        {
            return a.log != b.log;
        }

        public override bool Equals(object o)
        {
            var b = (LogData)o;
            return log == b.log;
        }

        public override int GetHashCode()  //??????????
        {
            return -1;
        }
        #endregion
        */
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
