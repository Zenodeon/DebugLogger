using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public LoggerWindow loggerWindow { get; set; }
        public TabPanel tabPanel { get; set; }

        private ScrollViewer logViewer { get; set; }

        ActiveLogList<ContentPresenter> activeLogs = new ActiveLogList<ContentPresenter>();

        private bool atSll = true;

        public bool autoScroll
        {
            get { return atSll; }
            private set
            {
                atSll = value;

                SEBtn.Visibility = value ? Visibility.Collapsed : Visibility.Visible;

                loggerWindow.ToggleAutoScrollButton(value);
            }
        }

        public bool ToggleAutoScroll()
        {
            return autoScroll = !autoScroll;
        }

        public LogPanel()
        {          
            InitializeComponent();

            logsControl.ApplyTemplate();
            logViewer = (ScrollViewer)logsControl.Template.FindName("logViewer", logsControl);

            activeLogs.Initialize();

            logsControl.ItemsSource = activeLogs;
        }

        private void LogsControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (autoScroll & e.Delta > 0)
                autoScroll = false;

            logViewer.ScrollToVerticalOffset(logViewer.VerticalOffset - (e.Delta / 6));

            e.Handled = true;
        }

        private void SEBtn_ScrollToEnd(object sender, RoutedEventArgs e)
        {
            logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight);

            autoScroll = true;
        }

        public void ClearLogs()
        {
            activeLogs.Clear();
        }

        public void ProcessLog(LogData logData)
        {
            LogMessage logM = CreateLogMessage(logData);
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

        public void DisplayLog(LogMessage logM)
        {
            activeLogs.Add(logM);

            if (autoScroll)
                logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight + logM.frame.Height);
        }

        public void DisplayLog(List<LogMessage> logMList)
        {
            activeLogs.Add(logMList);
        }

        public void RemoveLog(LogMessage logM)  
        {
            activeLogs.Remove(logM);
        }

        public void RemoveLog(List<LogMessage> logMList)
        {
            activeLogs.Remove(logMList);
        }
    }
}
