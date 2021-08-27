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

            tabPanel.logPanel = logPanel;
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
            Dispatcher.BeginInvoke(() => logPanel.DisplayLog(logData), DispatcherPriority.Background);
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
            logPanel.ClearLogs();
        }

        private void ToggleAutoScroll(object sender, RoutedEventArgs e)
        {
            logPanel.autoScroll = !logPanel.autoScroll;
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
    }
}

