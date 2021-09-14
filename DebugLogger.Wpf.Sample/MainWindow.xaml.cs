using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DebugLogger.Wpf.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DLog.Instantiate();

            ChangeIndicator(IndicatorState.Inactive);

            DLog.Log("Sample Project Running : Type 1");
            DLog.Log("Sample Project Running : Type 2");

            DLog.Log("Sample Project Running : Type 1");
            DLog.Log("Sample Project Running : Type 2");

            //TestKFormat();
        }

        private void TestKFormat()
        {
            //100 | 100
            DLog.Log(100.KFormat());
            DLog.Log(123.KFormat());

            //1000 | 1K
            DLog.Log(1000.KFormat());
            DLog.Log(1234.KFormat());

            //10000 | 10K
            DLog.Log(10000.KFormat());
            DLog.Log(12345.KFormat());

            //100000 100K
            DLog.Log(100000.KFormat());
            DLog.Log(123456.KFormat());

            //1000000 1M
            DLog.Log(1000000.KFormat());
            DLog.Log(1234567.KFormat());

            //10000000 10M
            DLog.Log(10000000.KFormat());
            DLog.Log(12345678.KFormat());

            //100000000 100M
            DLog.Log(100000000.KFormat());
            DLog.Log(123456789.KFormat());

            //1000000000 1000M
            DLog.Log(1000000000.KFormat());
            DLog.Log(1234567891.KFormat());
        }

        private void OnClosingWindow(object sender, CancelEventArgs e)
        {
            DLog.Close();
        }

        private void Log(object sender, RoutedEventArgs e)
        {
            DLog.Log(logMessage.Text);      
        }

        private void Warn(object sender, RoutedEventArgs e)
        {
            DLog.Warn(logMessage.Text);
        }

        private void Alert(object sender, RoutedEventArgs e)
        {
            DLog.Alert(logMessage.Text);
        }

        private void LogX(object sender, RoutedEventArgs e)
        {
            UpdateIndicator(IndicatorState.Active);

            int num = int.Parse(mutlipler.Text.Split(' ')[0]);

            string message = logMessage.Text;

            Task.Run(() =>
            {
                for (int i = 1; i <= num; i++)
                {
                    Thread.Sleep(10); //Load
                    DLog.Log(message + " : " + i);
                }

                UpdateIndicator(IndicatorState.Completed);
            });
        }

        private void UpdateIndicator(IndicatorState state)
        {
            if (Dispatcher.CheckAccess())
                ChangeIndicator(state);
            else
                Dispatcher.Invoke(() => ChangeIndicator(state));
        }

        private void ChangeIndicator(IndicatorState state)
        {
            switch (state)
            {
                case IndicatorState.Inactive:
                    Indicator.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    break;

                case IndicatorState.Active:
                    Indicator.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
                    break;

                case IndicatorState.Completed:
                    Indicator.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                    break;

                default:
                    break;
            }
        }

        private readonly Regex numericalRegex = new Regex("[^0-9]"); 

        private void mutlipler_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);
        }

        private void Lag(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(10000);
        }

        private enum IndicatorState
        {
            Inactive,
            Active,
            Completed
        }

        /* /// Same Hash Code Finder
         *    var words = new Dictionary<int, string>();

            int i = 0;
            string teststring;
            while (true)
            {
                i++;
                teststring = i.ToString();
                try
                {
                    words.Add(teststring.GetHashCode(), teststring);
                }
                catch (Exception)
                {
                    break;
                }
            }

            var collisionHash = teststring.GetHashCode();

            DLog.Log($"\"{words[collisionHash]}\" and \"{teststring}\" have the same hash code {collisionHash}");
*/
    }
}
