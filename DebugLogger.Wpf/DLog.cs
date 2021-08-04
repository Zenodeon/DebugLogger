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
        private static LoggerWindow logWindow;

        public static void Instantiate()
        {
            return;
            if (logWindow == null)
            {
                Thread newWindowThread = new Thread(new ThreadStart(NewLoggerWindow));
                newWindowThread.SetApartmentState(ApartmentState.STA);
                newWindowThread.IsBackground = true;
                newWindowThread.Start();
            }
        }

        private static void NewLoggerWindow()
        {
            LoggerWindow loggerWindow = new LoggerWindow();
            SetLogWindow(loggerWindow);
            //loggerWindow.Show();
            
            Dispatcher.Run();
            
        }

        public static void SetLogWindow(LoggerWindow window)
        {
            if (logWindow == null)
                logWindow = window;
            //else
            //window.Close();

            logWindow.Show();

            //ShowWindowSafe(window);
        }

        private static void ShowWindowSafe(Window w)
        {
            if (w.Dispatcher.CheckAccess())
                w.Show();
            else
                w.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(w.Show));
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
