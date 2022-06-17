namespace AutoCloseWithTimer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        #region Fields

        private const string TimeSpanFormat = @"hh\:mm\:ss";

        private const double MaxTimeDifferenceMinutes = 10.0;

        private const double SecondsToClose = 5.0;

        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private TimeSpan workingTimeEnd;

        private List<WorkTimeRecord> records = new List<WorkTimeRecord>
        {
            WorkTimeRecord.Create(1, TimeSpan.FromHours(10), new TimeSpan(16, 5, 0)),
            WorkTimeRecord.Create(2, null, null),
            WorkTimeRecord.Create(3, TimeSpan.FromHours(17.5), TimeSpan.FromHours(20)),
            WorkTimeRecord.Create(1, null, TimeSpan.FromHours(17)),
        };

        #endregion


        public MainWindow()
        {
            InitializeComponent();

            KeyUp += MainWindow_KeyUp;
            Closing += MainWindow_Closing;

            var workTimes = records
                .Where(i => i.HasWorkingTime)
                .ToList();

            ManageWorkTimeAtStart(workTimes);

            ManageWorkTimeEnd(workTimes);

            var times = workTimes.Select(i => $"Работно Време {i.Id}: От {i.FromTime.Value.ToString(TimeSpanFormat)} До {i.ToTime.Value.ToString(TimeSpanFormat)} часа;");
            textBoxData.Text = string.Join(Environment.NewLine, times);
        }


        #region Event Handlers

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CloseApp();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            dispatcherTimer.Tick -= DispatcherTimer_Tick;

            MessageBox.Show(
                $"Работното ви време изтече.{Environment.NewLine}Приложението ще се затвори след {SecondsToClose} секунди.",
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            dispatcherTimer.Tick += delegate { CloseApp(); };
            dispatcherTimer.Interval = TimeSpan.FromSeconds(SecondsToClose);
            dispatcherTimer.Start();
        }

        #endregion


        #region Helpers

        private void CloseApp()
        {
            Application.Current.Shutdown(0);
        }

        private void ManageWorkTimeAtStart(List<WorkTimeRecord> workTimes)
        {
            var span = DateTime.Now.TimeOfDay;

            var result = workTimes
                .Any(i => i.FromTime.Value <= span && span <= i.ToTime.Value);

            if (!result)
            {
                MessageBox.Show(
                    "Не можете да използвате приложението извън работно време.",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                CloseApp();
            }
        }

        private void ManageWorkTimeEnd(List<WorkTimeRecord> workTimes)
        {
            var beforeSpan = TimeSpan.FromSeconds(SecondsToClose);
            var span = DateTime.Now.TimeOfDay;

            var max = workTimes
                .Where(i => i.FromTime.Value <= span && span <= i.ToTime.Value)
                .Max(i => i.ToTime.Value);

            workingTimeEnd = max;

            var endTime = max - span;
            var notificationTime = endTime - beforeSpan;

            txtStatus.Text = $"Работното ви време ще изтече в {max} след {endTime.ToString(TimeSpanFormat)} часа. Нотификация след {notificationTime.ToString(TimeSpanFormat)} часа.";

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = notificationTime;
            dispatcherTimer.Start();
        }

        #endregion
    }
}
