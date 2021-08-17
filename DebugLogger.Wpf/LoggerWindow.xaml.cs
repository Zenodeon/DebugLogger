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

                if (value)
                {
                    Resources["ToggleCurrent"] = Resources["ToggleOn"];
                    SEBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Resources["ToggleCurrent"] = Resources["ToggleOff"];
                    SEBtn.Visibility = Visibility.Visible;
                }
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
        //Start Region -----------------------------------------------------------------------------------------
        //Start Region -----------------------------------------------------------------------------------------
        //Start Region -----------------------------------------------------------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logList.Items.Clear();
            logBase.Clear();
        }

        private void ToggleAutoScroll(object sender, RoutedEventArgs e)
        {
            autoScroll = !autoScroll;
        }
        private void ScrollToEnd(object sender, RoutedEventArgs e)
        {
            logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight);
            autoScroll = true;
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
            if (autoScroll & e.Delta > 0)
                autoScroll = false;
               
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

        //End Region -----------------------------------------------------------------------------------------
        //End Region -----------------------------------------------------------------------------------------
        //End Region -----------------------------------------------------------------------------------------
        #endregion

        private void NewTab(string name)
        {
            ContentPresenter tabFrame = new ContentPresenter();
            LogTab tab = new LogTab(name);

            tabFrame.Width = tab.Width;
            //tabFrame.Height = tab.Height;

            tabFrame.Content = tab;

            tabList.Items.Add(tabFrame);
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
                    logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight);

                logBase.Add(logData.log, logMessage);
            }
        }
    }
}

