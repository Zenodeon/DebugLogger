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
    /// Interaction logic for LogMessage.xaml
    /// </summary>
    public partial class LogMessage : Page
    {
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

        public LogMessage(int index, string log)
        {
            InitializeComponent();

            Index.Content = index;
            LogBox.Content = log;
            logCount = 1;
        }
    }
}
