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
        private List<LogMessage> logs = new List<LogMessage>();

        public LoggerWindow()
        {
            InitializeComponent();

            DLog.LoggerWindowInstantiated(this);

            tabPanel.logPanel = logPanel;

            logPanel.loggerWindow = this;
            logPanel.tabPanel = tabPanel;

            foreach (DefaultLogType name in (DefaultLogType[])Enum.GetValues(typeof(DefaultLogType)))
                tabPanel.CreateTab(name);
        }

        /*
        public void ReportLog(LogData logData)
        {
            Dispatcher.BeginInvoke(() => NewLog(logData), DispatcherPriority.Background);
        }*/

        public void ReportLog(LogData logData)
        {
            Dispatcher.BeginInvoke(() => logPanel.ProcessLog(logData), DispatcherPriority.Normal);
        }

        /*
        private void NewLog(LogData logData)
        {
            if(tabList.ContainsKey(logData.LogType))
                tabList[logData.LogType].Add(logData);
            else
                CreateTab(logData.LogType);
        }*/

        #region UI Event

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logPanel.ClearAllLogs();
        }

        private void ToggleAutoScroll(object sender, RoutedEventArgs e)
        {
            logPanel.ToggleAutoScroll();
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

        public void ToggleAutoScrollButton(bool active)
        {
            Resources["ToggleCurrent"] = active ? Resources["ToggleOn"] : Resources["ToggleOff"];
        }
    }
}

