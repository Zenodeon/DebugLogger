using System;
using System.Collections.Generic;
using System.Text;

namespace DebugLogger.Wpf
{
    public static class DLog
    {
        private static LoggerWindow logWindow;

        public static void Instantiate()
        {
            if (logWindow == null)
                logWindow = new LoggerWindow();

            logWindow.Show();
        }

        public static void Close()
        {
            if (logWindow != null)
                logWindow.Close();
        }

        public static void Log(string log)
        {
            if (logWindow != null)
                logWindow.ReportLog(log);
        }
    }
}
