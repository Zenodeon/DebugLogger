using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace DebugLogger.Wpf
{
    public static class DLog
    {
        private static bool instantiated = false;
        private static LoggerWindow logWindow;

        private static ManualResetEvent signal = new ManualResetEvent(false);

        public static bool Instantiate()
        {
            if (instantiated)
                return true;

            Thread logThread = new Thread(new ThreadStart(() =>
            {
                LoggerWindow loggerWindow = new LoggerWindow();
                loggerWindow.Show();

                Dispatcher.Run();
            }));

            logThread.SetApartmentState(ApartmentState.STA);
            logThread.IsBackground = true;
            logThread.Start();

            instantiated = true;

            signal.WaitOne();

            return true;
        }

        public static void LoggerWindowInstantiated(LoggerWindow loggerWindow)
        {
            logWindow = loggerWindow;
            signal.Set();
        }

        public static void Close()
        {
            if (logWindow != null)
                logWindow.CloseWindow();
        }

        public static void Log(string log)
        {
            if (logWindow != null)
                logWindow.ReportLog(new LogData(DefaultLogType.Master, log));
        }

        public static void Warn(string log)
        {
            if (logWindow != null)
                logWindow.ReportLog(new LogData(DefaultLogType.Warning, log));
        }

        public static void Alert(string log)
        {
            if (logWindow != null)
                logWindow.ReportLog(new LogData(DefaultLogType.Error, log));
        }
    }
}
