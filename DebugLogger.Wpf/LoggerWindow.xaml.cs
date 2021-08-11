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

            int countt = 1;

            for(int i = 0; i < countt; i++)
                test();

        }

        private void test()
        {
            Frame frame = new Frame();
            LogTab tab = new LogTab();

            frame.Width = tab.Width + 1;

            frame.Content = tab;

            tabList.Items.Add(frame);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logList.Items.Clear();
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

        private void TabList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tabViewer.ScrollToHorizontalOffset(tabViewer.HorizontalOffset - (e.Delta / 5));
            e.Handled = true;
        }

        private void UpdateLogWindowWidth()
        {
            foreach (KeyValuePair<string, LogMessage> key in logBase)
            {
                LogMessage lm = key.Value;

                lm.frame.Width = lm.Width = logList.ActualWidth;
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

                frame.Width = logMessage.Width = logList.ActualWidth;
                frame.Height = logMessage.Height + 1;

                frame.Content = logMessage;

                logList.Items.Add(frame);

                //logItem.ScrollIntoView(frame);

                logBase.Add(log, logMessage);
            }
        }

        public void CloseWindow()
        {
            Dispatcher.BeginInvoke(() => Close(), DispatcherPriority.Background);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Bar_Mouse(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /*
        <ScrollViewer Name = "logViewer" Grid.Row="2" 
                 VerticalScrollBarVisibility="Hidden" 
                 HorizontalScrollBarVisibility="Disabled">
            <ListBox Name = "logList" ScrollViewer.CanContentScroll="False" PreviewMouseWheel="LogList_PreviewMouseWheel" Background="#1c1c1c" BorderThickness="0" 
                 ScrollViewer.VerticalScrollBarVisibility="Disabled" 
                 ScrollViewer.HorizontalScrollBarVisibility="Disabled"/>
        </ScrollViewer>
        */
    }
}

