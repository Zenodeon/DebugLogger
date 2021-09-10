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

        public Enum type;

        private List<LogMessage> logs = new List<LogMessage>();

        private bool _active = true;

        public bool active
        {
            get { return _active; }
            set
            {
                _active = value;

                if (active)
                    logPanel.DisplayLog(logs);
                else
                    logPanel.RemoveLog(logs);

                TabActiveShade.Opacity = value ? 0 : 0.5;
            }
        }

        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            active = !active;
        }

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

                Count.Content = value.KFormat();
            }
        }

        public LogTab()
        {
            InitializeComponent();
        }

        public LogTab(Enum tabName)
        {
            type = tabName;

            InitializeTab(tabName.ToString());
        }

        public LogTab(string tabName)
        {
            InitializeTab(tabName);
        }

        private void InitializeTab(string tabName)
        {
            InitializeComponent();

            //bool isDefault = tabName == DefaultLogType.Master.ToString() | tabName == DefaultLogType.Warning.ToString() | tabName == DefaultLogType.Error.ToString();
            bool isDefault = tabName == DefaultLogType.Master.ToString();

            if (isDefault)
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

        public void AddLogMessage(LogMessage logM)
        {
            logs.Add(logM);
            logCount++;

            if (active)
                logPanel.DisplayLog(logM);
        }
    }    
}
