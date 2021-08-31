using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class LogTab : UserControl
    {
        public LogPanel logPanel { get; set; }
        public ContentPresenter frame { get; set; }

        private bool _active = true;

        public bool active
        {
            get { return _active; }
            set
            {
                _active = value;

                TabActiveShade.Opacity = value ? 0 : 0.5;                 
            }
        }

        //private Dictionary<int, LogMessage> logs = new Dictionary<int, LogMessage>();

        private List<LogMessage> logs = new List<LogMessage>();

        private int count = 0;
        private int logCount
        {
            get
            {
                return count;
            }
            set
            {
                count = value;

                Count.Content = value;
            }
        }

        public LogTab()
        {
            InitializeComponent();
        }

        public LogTab(Enum tabName)
        {
            InitializeTab(tabName.ToString());
        }

        public LogTab(string tabName)
        {
            InitializeTab(tabName);
        }

        private void InitializeTab(string tabName)
        {
            InitializeComponent();

            bool isMaster = tabName == DefaultLogType.Master.ToString();

            if (isMaster)
            {
                tabName = "";
                mainGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
            }

            tabText.Text = tabName;

            UpdateTabSize();
        }

        public void UpdateTabSize()
        {
            FormattedText formattedText = new FormattedText(
                tabText.Text,
                CultureInfo.CurrentCulture,
                tabText.FlowDirection,
                new Typeface(tabText.FontFamily, tabText.FontStyle, tabText.FontWeight, tabText.FontStretch),
                tabText.FontSize,
                tabText.Foreground, VisualTreeHelper.GetDpi(this).PixelsPerDip);

            double newTextBoxWidth = formattedText.WidthIncludingTrailingWhitespace;

            Width = GetGridWidth(Grid.GetColumn(tabBox)) + newTextBoxWidth;

            tabBox.Width = newTextBoxWidth;
        }

        public double GetGridWidth(int dontIncluedCol = -1)
        {
            double width = 0;

            for (int i = 0; i < mainGrid.ColumnDefinitions.Count; i++)
                if(i != dontIncluedCol) 
                    width += mainGrid.ColumnDefinitions[i].Width.Value;

            return width;
        }


        private void Tab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //DLog.Log(e.);
        }
        private void Tab_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //DLog.Log(e.ButtonState.ToString());
        }

        public void AddLogMessage(LogMessage logM)
        {
            logs.Add(logM);
            logCount++;
        }

        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            active = !active;
        }
    }    
}
