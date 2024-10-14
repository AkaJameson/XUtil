namespace XUtil.Core.PeriodTask
{
    public class WeeklyTaskRunner : IPeriodTask
    {
        private readonly Func<Task> _taskToRun;
        private readonly DayOfWeek[] _daysOfWeekToRun; // 运行任务的星期几数组
        private readonly TimeSpan _dailyRunTime;       // 运行任务的时间
        private CancellationTokenSource _cancellationTokenSource;
        private Task _runningTask;

        public WeeklyTaskRunner(Func<Task> taskToRun, DayOfWeek[] daysOfWeekToRun, TimeSpan dailyRunTime)
        {
            _taskToRun = taskToRun ?? throw new ArgumentNullException(nameof(taskToRun));
            _daysOfWeekToRun = daysOfWeekToRun ?? throw new ArgumentNullException(nameof(daysOfWeekToRun));
            _dailyRunTime = dailyRunTime;
        }

        public void Start()
        {
            if (_runningTask != null)
            {
                throw new InvalidOperationException("已经运行.");
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _runningTask = RunWeeklyTaskAsync(_cancellationTokenSource.Token);
            return;
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _runningTask = null;
            }
            return;
        }

        private async Task RunWeeklyTaskAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var nextRunDelay = GetNextRunDelay();
                    Console.WriteLine($"下次运行时间: {nextRunDelay}");
                    await Task.Delay(nextRunDelay, cancellationToken); // 等待下一次运行时间到来
                    await _taskToRun();
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"未知异常: {ex.Message}");
                }
            }
        }

        private TimeSpan GetNextRunDelay()
        {
            var now = DateTime.Now;

            var nextRunTime = new DateTime(now.Year, now.Month, now.Day,
                                           _dailyRunTime.Hours, _dailyRunTime.Minutes, _dailyRunTime.Seconds);
            int daysUntilNextRun = 7;

            foreach (var dayOfWeek in _daysOfWeekToRun)
            {
                int daysUntilDay = ((int)dayOfWeek - (int)now.DayOfWeek + 7) % 7;

                if (daysUntilDay == 0 && now.TimeOfDay > _dailyRunTime)
                {
                    daysUntilDay += 7;
                }

                if (daysUntilDay < daysUntilNextRun)
                {
                    daysUntilNextRun = daysUntilDay;
                    nextRunTime = nextRunTime.AddDays(daysUntilNextRun);
                }
            }

            return nextRunTime - now;
        }
    }

}
