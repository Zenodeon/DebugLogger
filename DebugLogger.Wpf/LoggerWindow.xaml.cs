using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        private int logWindowCount = 0;

        private Dictionary<string, LogMessage> logBase = new Dictionary<string, LogMessage>();

        private Stopwatch watch = new Stopwatch();

        public LoggerWindow()
        {
            InitializeComponent();

            watch.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogList.Items.Clear();
            logBase.Clear();
            logWindowCount = 0;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLogWindowWidth();
        }

        private void LogList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            logViewer.ScrollToVerticalOffset(logViewer.VerticalOffset - (e.Delta / 5));
            e.Handled = true;
        }

        private void UpdateLogWindowWidth()
        {
            foreach (KeyValuePair<string, LogMessage> key in logBase)
            {
                LogMessage lm = key.Value;

                lm.frame.Width = lm.Width = LogList.ActualWidth - 10;
            }
        }

        public void ReportLog(string log)
        {
            return;
            Dispatcher.Invoke(() =>
            {
                NewLog(log);
                //return;
                Thread logThread = new Thread(new ThreadStart(() =>
                {
                    NewLog(log);

                    Dispatcher.Run();
                }));

                logThread.SetApartmentState(ApartmentState.STA);
                logThread.IsBackground = true;
                logThread.Start();
            });
        }

        private void NewLog(string log)
        {
            if (logBase.ContainsKey(log))
                logBase[log].logCount++;
            else
            {
                Frame frame = new Frame();
                LogMessage logMessage = new LogMessage(logWindowCount++, log);
                //LogMessage logMessage = new LogMessage(watch.Elapsed.TotalSeconds, log);
                logMessage.frame = frame;

                frame.Width = logMessage.Width = LogList.ActualWidth - 10;

                frame.Content = logMessage;



                //test(frame, log, logMessage);



                    Dispatcher.Invoke(() =>
                    {

                        test(frame, log, logMessage);


                    });
            }
        }

        private void test(Frame f, string s, LogMessage l)
        {
                    LogList.Items.Add(f);
                LogList.ScrollIntoView(f);

                logBase.Add(s, l);

        }
    }
}

