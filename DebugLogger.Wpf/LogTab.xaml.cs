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
        public Frame tabFrame { get; set; }

        //public List<>

        public LogTab()
        {
            InitializeComponent();

        }

        public LogTab(string tabName)
        {
            InitializeComponent();

            tabText.Text = tabName;

            if (tabName == "")
                mainGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);

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

            //DLog.Log(formattedText.WidthIncludingTrailingWhitespace + "");
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
            DLog.Log(e.ButtonState.ToString());
        }
    }    
}
