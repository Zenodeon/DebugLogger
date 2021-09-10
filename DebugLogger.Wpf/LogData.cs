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

        public DateTime occurred { get { return occasions[0]; } private set { occasions[0] = value; } }
        public List<DateTime> occasions { get; private set; } 
        public DateTime latestOccurrence => occasions[logCount - 1];

        public int logCount => occasions.Count;

        public LogData(DefaultLogType type, string log)
        {
            occasions = new List<DateTime> { DateTime.Now };

            logType = type;
            this.log = log;
        }

        public void OffsetOccurredTick()
        {
            occurred = occurred.AddTicks(1);
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
            a.occasions = a.occasions.Concat(b.occasions).ToList();

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
}
