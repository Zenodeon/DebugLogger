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
    /// <summary>
    /// Interaction logic for LogMessage.xaml
    /// </summary>
    public partial class LogMessage : UserControl
    {
        public ContentPresenter frame { get; set; }

        public LogData logData { get; set; }

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

        public LogMessage()
        {
            InitializeComponent();
        }

        public LogMessage(LogData logData)
        {
            InitializeComponent();

            ShowLog(logData);

            logCount = 1;
        }

        public void ShowLog(LogData logData)
        {
            this.logData = logData;

            TextRange timePeriodText = new TextRange(LogBox.Document.ContentStart, LogBox.Document.ContentEnd);
            timePeriodText.Text = logData.latestOccurrence.defaultFormat() + " ";
            timePeriodText.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 100, 100, 100)));

            TextRange logText = new TextRange(LogBox.Document.ContentEnd, LogBox.Document.ContentEnd);
            logText.Text = logData.log;
            logText.ApplyPropertyValue(TextElement.ForegroundProperty, GetLogColor(logData.logType));

            if (logData.logTypeS != DefaultLogType.Master.ToString())
                Border.Background = GetLogColor(logData.logType).Brightness(25);
        }

        private SolidColorBrush GetLogColor(Enum type)
        {
            switch (type)
            {
                case DefaultLogType.Master:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); //White

                case DefaultLogType.Warning:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)); //Yellow

                case DefaultLogType.Error:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)); //Red

                default:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); //White
            }
        }

       
    }

    public static class TempExtentsion
    {
        public static SolidColorBrush Brightness(this SolidColorBrush brush, double percentage)
        {
            percentage = Math.Clamp(percentage, 0, 100) / 100;
            byte r = (byte)(int)(brush.Color.R * percentage);
            byte g = (byte)(int)(brush.Color.G * percentage);
            byte b = (byte)(int)(brush.Color.B * percentage);

            return new SolidColorBrush(Color.FromArgb(brush.Color.A, r, g, b));
        }
    }
}
