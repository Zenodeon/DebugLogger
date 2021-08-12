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

        public static void Instantiate()
        {
            if (!instantiated)
            {
                Thread logThread = new Thread(new ThreadStart(() =>
                {
                    LoggerWindow loggerWindow = new LoggerWindow();

                    logWindow = loggerWindow;
                    logWindow.Show();

                    Dispatcher.Run(); 
                }));

                logThread.SetApartmentState(ApartmentState.STA);
                logThread.IsBackground = true;
                logThread.Start();

                instantiated = true;
            }
        }

        public static void Close()
        {
            if (logWindow != null)
                logWindow.CloseWindow();
        }

        public static void Log(string log)
        {
            if (logWindow != null)
                logWindow.ReportLog(log);
        }
    }

    public enum LogType
    {
        Warning,
        Error
    }
}
