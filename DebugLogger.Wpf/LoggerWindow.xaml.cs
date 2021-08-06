using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace DebugLogger.Wpf
{
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        private Dictionary<string, LogMessage> logBase = new Dictionary<string, LogMessage>();

        private Stopwatch watch = new Stopwatch();

        public LoggerWindow()
        {
            InitializeComponent();

            watch.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogList.Items.Clear();
            logBase.Clear();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLogWindowWidth();
        }

        private void LogList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            logViewer.ScrollToVerticalOffset(logViewer.VerticalOffset - (e.Delta / 5));
            e.Handled = true;
        }

        private void UpdateLogWindowWidth()
        {
            foreach (KeyValuePair<string, LogMessage> key in logBase)
            {
                LogMessage lm = key.Value;

                lm.frame.Width = lm.Width = LogList.ActualWidth - 10;
            }
        }

        public void ReportLog(string log)
        {
            Dispatcher.BeginInvoke(() => NewLog(log), DispatcherPriority.Background);
        }

        private void NewLog(string log)
        {
            DateTime time = DateTime.Now;

            if (logBase.ContainsKey(log))
                logBase[log].logCount++;
            else
            {
                Frame frame = new Frame();
                LogMessage logMessage = new LogMessage(time, log);

                logMessage.frame = frame;

                frame.Width = logMessage.Width = LogList.ActualWidth - 10;

                frame.Content = logMessage;

                LogList.Items.Add(frame);
                LogList.ScrollIntoView(frame);

                logBase.Add(log, logMessage);
            }
        }
    }
}

