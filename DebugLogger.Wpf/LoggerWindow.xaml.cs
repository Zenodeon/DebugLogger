using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        private int logWindowCount = 0;

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
            logWindowCount = 0;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLogWindowWidth();
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
            Dispatcher.Invoke(() =>
            {
                NewLog(log);
            });
        }

        private void NewLog(string log)
        {
            if (logBase.ContainsKey(log))
                logBase[log].logCount++;
            else
            {
                Frame frame = new Frame();
                LogMessage logMessage = new LogMessage(logWindowCount++, log);
                //LogMessage logMessage = new LogMessage(watch.Elapsed.TotalSeconds, log);
                logMessage.frame = frame;

                frame.Width = logMessage.Width = LogList.ActualWidth - 10;

                frame.Content = logMessage;

                LogList.Items.Add(frame);

                logBase.Add(log, logMessage);

                LogList.ScrollIntoView(frame);
            }
        }
    }
}


/* Drag System, Not smooth >:(
                PreviewMouseDown="LogList_PreviewMouse"
                PreviewMouseUp="LogList_PreviewMouse"
                PreviewMouseMove="LogList_PreviewMouseMove"
 
        private MouseButtonState llMouseState = MouseButtonState.Released;
        private double llMousePosY = 0;

        private void LogList_PreviewMouse(object sender, MouseButtonEventArgs e)
        {
            llMouseState = e.ButtonState;
            llMousePosY = e.GetPosition(LogList).Y;
        }

        private void LogList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (llMouseState == MouseButtonState.Pressed)
                UpdateScrollView(e.GetPosition(LogList).Y);
        }

        private void UpdateScrollView(double dir)
        {
            DLog.Log(dir + "");

            Border border = (Border)VisualTreeHelper.GetChild(LogList, 0);
            ScrollViewer scrollViewer = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
            if (scrollViewer != null)
            {
                //scrollViewer.ScrollToVerticalOffset(dir/10);
                //scrollViewer.scroll
            }
        }
*/

