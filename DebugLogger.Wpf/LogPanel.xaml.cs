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

        private Dictionary<string, LogMessage> activeLog = new Dictionary<string, LogMessage>();

        BindingList<ContentPresenter> tList = new BindingList<ContentPresenter>();

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

            tList.AllowEdit = true;
            tList.AllowNew = true;
            tList.AllowRemove = true;

            logsControl.ItemsSource = tList;
        }

        private void logsControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
            logsControl.Items.Clear();
            activeLog.Clear();
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
            tList.Add(logM.frame);
           
            if (autoScroll)
               logViewer.ScrollToVerticalOffset(logViewer.ScrollableHeight + logM.frame.Height);
        }
        public void DisplayLogs(List<LogMessage> logMList)
        {
            tList.RaiseListChangedEvents = false;
            foreach (LogMessage log in logMList)
                DisplayLog(log);

            tList.RaiseListChangedEvents = true;
            tList.ResetBindings();
        }

        public void RemoveLog(LogMessage logM)  
        {
            tList.Remove(logM.frame);
        }

        public void RemoveLogs(List<LogMessage> logMList)
        {
            tList.RaiseListChangedEvents = false;

            foreach (LogMessage log in logMList)
                RemoveLog(log);

            tList.RaiseListChangedEvents = true;
            tList.ResetBindings();
        }
    }
}
