using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DebugLogger.Wpf
{
    /// <summary>
    /// Interaction logic for LogPanel.xaml
    /// </summary>
    public partial class LogPanel : UserControl
    {
        public TabPanel tabPanel { get; set; }

        private ScrollViewer logViewer { get; set; }

        private Dictionary<string, LogMessage> activeLog = new Dictionary<string, LogMessage>();

        private bool atSll = true;

        public bool autoScroll
        {
            get { return atSll; }
            set
            {
                atSll = value;

                if (value)
                {
                    //Resources["ToggleCurrent"] = Resources["ToggleOn"];
                    SEBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Resources["ToggleCurrent"] = Resources["ToggleOff"];
                    SEBtn.Visibility = Visibility.Visible;
                }
            }
        }

        public LogPanel()
        {          
            InitializeComponent();

            logsControl.ApplyTemplate();
            logViewer = (ScrollViewer)logsControl.Template.FindName("logViewer", logsControl);
        }

        private void logsControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (autoScroll & e.Delta > 0)
                autoScroll = false;

            logViewer.ScrollToVerticalOffset(logViewer.VerticalOffset - (e.Delta / 5));

            e.Handled = true;
        }

        private void SEBtn_ScrollToEnd(object sender, RoutedEventArgs e)
        {
            logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight);
            autoScroll = true;
        }

        public void ClearLogs()
        {
            logsControl.Items.Clear();
            activeLog.Clear();
        }

        /*
        public void DisplayLog(LogData logData)
        {
            if (activeLog.ContainsKey(logData.log))
                activeLog[logData.log].logCount++;
            else
            {
                LogMessage logM = CreateLogMessage(logData);

                logsControl.Items.Add(logM.frame);

                if (autoScroll)
                    logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight);

                activeLog.Add(logData.log, logM);
            }
        }*/

        public void DisplayLog(LogData logData)
        {
            LogMessage logM = CreateLogMessage(logData);

            logsControl.Items.Add(logM.frame);

            if (autoScroll)
                logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight);

            tabPanel.tabs[logData.logType].AddLogMessage(logM);
        }

        private LogMessage CreateLogMessage(LogData logData)
        {
            ContentPresenter frame = new ContentPresenter();
            LogMessage log = new LogMessage(logData);

            log.frame = frame;

            frame.Width = log.Width = double.NaN;
            frame.Height = log.Height + 1;

            frame.Content = log;

            return log;
        }
    }
}
