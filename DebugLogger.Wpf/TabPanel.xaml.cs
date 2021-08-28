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
    /// Interaction logic for TabPanel.xaml
    /// </summary>
    public partial class TabPanel : UserControl
    {
        public LogPanel logPanel { get; set; }

        private ScrollViewer tabViewer { get; set; }

        public Dictionary<Enum, LogTab> tabs = new Dictionary<Enum, LogTab>();

        public TabPanel()
        {
            InitializeComponent();

            tabsControl.ApplyTemplate();
            tabViewer = (ScrollViewer)tabsControl.Template.FindName("tabViewer", tabsControl);
        }

        public void CreateTab(Enum tabName)
        {
            ContentPresenter tabFrame = new ContentPresenter();
            LogTab tab = new LogTab(tabName);

            tab.logPanel = logPanel;
            tab.frame = tabFrame;

            tabFrame.Width = tab.Width;
            tabFrame.Height = tab.Height;
            tabFrame.Content = tab;

            tabs.Add(tabName, tab);
            tabsControl.Items.Add(tabFrame);
        }

        private void TabList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tabViewer.ScrollToHorizontalOffset(tabViewer.HorizontalOffset - (e.Delta / 5));
            e.Handled = true;
        }
    }
}
