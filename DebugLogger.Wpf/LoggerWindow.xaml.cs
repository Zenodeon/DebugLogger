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
        private Dictionary<Enum, LogTab> tabList = new Dictionary<Enum, LogTab>();
        
        private List<LogMessage> logs = new List<LogMessage>();

        private ScrollViewer tv;
        private ScrollViewer tabViewer
        {
            get
            {
                if (tv == null)
                    tv = (ScrollViewer)tabPanel.Template.FindName("tabViewer", tabPanel);

                return tv;
            }
        }

        #region UI Event
        //Start Region -----------------------------------------------------------------------------------------
        //Start Region -----------------------------------------------------------------------------------------
        //Start Region -----------------------------------------------------------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logPanel.ClearLogs();
        }

        private void ToggleAutoScroll(object sender, RoutedEventArgs e)
        {
            logPanel.autoScroll = !logPanel.autoScroll;
        }

        private void TabList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tabViewer.ScrollToHorizontalOffset(tabViewer.HorizontalOffset - (e.Delta / 5));
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

        public LoggerWindow()
        {
            InitializeComponent();

            foreach (DefaultLogType type in (DefaultLogType[])Enum.GetValues(typeof(DefaultLogType)))
                CreateTab(type);
        }

        private void CreateTab(Enum tabName)
        {
            ContentPresenter tabFrame = new ContentPresenter();
            LogTab tab = new LogTab(tabName);

            tab.frame = tabFrame;
            //tab.logPanel = logPanel;

            tabFrame.Width = tab.Width;
            tabFrame.Height = tab.Height;
            tabFrame.Content = tab;          

            tabList.Add(tabName, tab);
            tabPanel.Items.Add(tabFrame);
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

    }
}

