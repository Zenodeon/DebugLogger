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
    public partial class LoggerWindow : Window
    {
        private bool atSll = true;
        private bool autoScroll
        {
            get
            {
                return
                    atSll;
            }
            set 
            {
                atSll = value;

                if(value)
                    Resources["ToggleCurrent"] = Resources["ToggleOn"];
                else
                    Resources["ToggleCurrent"] = Resources["ToggleOff"];
            }
        }

        private Dictionary<string, LogMessage> logBase = new Dictionary<string, LogMessage>();

        private ScrollViewer lv;
        private ScrollViewer logViewer 
        {
            get
            {
                if (lv == null)
                    lv = (ScrollViewer)logList.Template.FindName("logViewer", logList);

                return lv;
            }
        }

        private ScrollViewer tv;
        private ScrollViewer tabViewer
        {
            get
            {
                if (tv == null)
                    tv = (ScrollViewer)tabList.Template.FindName("tabViewer", tabList);

                return tv;
            }
        }

        public LoggerWindow()
        {
            InitializeComponent();

            NewTab("");

            foreach (DefaultLogType type in (DefaultLogType[])Enum.GetValues(typeof(DefaultLogType)))
                NewTab(type.ToString());
        }

        #region UI Event
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logList.Items.Clear();
            logBase.Clear();
        }

        private void ToggleAutoScroll(object sender, RoutedEventArgs e)
        {
            autoScroll = !autoScroll;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLogWindowWidth();
        }

        private void TabList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tabViewer.ScrollToHorizontalOffset(tabViewer.HorizontalOffset - (e.Delta / 5));
            e.Handled = true;
        }

        private void LogList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            logViewer.ScrollToVerticalOffset(logViewer.VerticalOffset - (e.Delta / 5));

            e.Handled = true;
        }

        private void Bar_Mouse(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void CloseWindow()
        {
            Dispatcher.BeginInvoke(() => Close(), DispatcherPriority.Background);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        private void NewTab()
        {
            Frame frame = new Frame();
            LogTab tab = new LogTab();

            frame.Width = tab.Width + 1;

            frame.Content = tab;

            tabList.Items.Add(frame);
        }

        private void NewTab(string name)
        {
            Frame frame = new Frame();
            LogTab tab = new LogTab(name);

            frame.Width = tab.Width + 1;

            frame.Content = tab;

            tabList.Items.Add(frame);
        }

       

        private void UpdateLogWindowWidth()
        {
            foreach (KeyValuePair<string, LogMessage> key in logBase)
            {
                LogMessage lm = key.Value;

                lm.frame.Width = lm.Width = logList.ActualWidth;
            }
        }

        public void ReportLog(LogData logData)
        {
            Dispatcher.BeginInvoke(() => NewLog(logData), DispatcherPriority.Background);
        }

        private void NewLog(LogData logData)
        {
            if (logBase.ContainsKey(logData.log))
                logBase[logData.log].logCount++;
            else
            {
                ContentPresenter frame = new ContentPresenter();
                LogMessage logMessage = new LogMessage(logData);

                //logMessage.frame = frame;

                frame.Width = logMessage.Width = logList.ActualWidth;
                frame.Height = logMessage.Height + 1;

                frame.Content = logMessage;

                logList.Items.Add(frame);

                if (autoScroll)
                    logViewer.ScrollToBottom();

                logBase.Add(logData.log, logMessage);
            }
        }
    }
}

