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
    public partial class LogTab : Page
    {
        public Frame frame { get; set; }

        private int count = 0;
        public int logCount
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

        public LogTab(DateTime time, string log)
        {
            InitializeComponent();

            string timePeriod =  $"[{time.ToString("hh:mm:ss:ff")}]";
            
            TextRange timePeriodText = new TextRange(tabBox.Document.ContentStart, tabBox.Document.ContentEnd);
            timePeriodText.Text = timePeriod + "  ";
            timePeriodText.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 100, 100, 100)));

            TextRange logText = new TextRange(tabBox.Document.ContentEnd, tabBox.Document.ContentEnd);
            logText.Text = log;
            logText.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)));

            logCount = 1;
        }

        private void TabBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (tabText == null)
                return;

            DLog.Log(TranslatePoint(new Point(0, 0), tabBox) + " : Before");


            FormattedText formattedText = new FormattedText(
                tabText.Text,
                CultureInfo.CurrentCulture,
                tabDoc.FlowDirection,
                new Typeface(tabDoc.FontFamily, tabDoc.FontStyle, tabDoc.FontWeight, tabDoc.FontStretch),
                tabDoc.FontSize,
                tabDoc.Foreground, VisualTreeHelper.GetDpi(this).PixelsPerDip);

            DLog.Log(tabBox.Width + " : Before");
            //tabBox.Width = formattedText.WidthIncludingTrailingWhitespace;
            DLog.Log(tabBox.Width + " : After");

            DLog.Log(formattedText.WidthIncludingTrailingWhitespace + "");
        }


    }    
}
