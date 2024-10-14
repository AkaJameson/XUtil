namespace XUtil.Core.PeriodTask
{
    public class MutiWeeklyTaskRunner : IPeriodTask
    {
        private readonly Func<Task> _taskToRun;
        private readonly List<DayOfWeek> _daysOfWeekToRun;  // 每周哪些天运行任务
        private readonly TimeSpan _dailyStartTime;          // 每天开始执行任务的时间
        private readonly TimeSpan _executionInterval;       // 执行任务的间隔
        private readonly int _executionCountPerDay;         // 每天执行的次数
        private CancellationTokenSource _cancellationTokenSource;
        private Task _runningTask;

        public MutiWeeklyTaskRunner(Func<Task> taskToRun, List<DayOfWeek> daysOfWeekToRun, TimeSpan dailyStartTime, TimeSpan executionInterval, int executionCountPerDay)
        {
            _taskToRun = taskToRun ?? throw new ArgumentNullException(nameof(taskToRun));
            _daysOfWeekToRun = daysOfWeekToRun ?? throw new ArgumentNullException(nameof(daysOfWeekToRun));
            _dailyStartTime = dailyStartTime;
            _executionInterval = executionInterval;
            _executionCountPerDay = executionCountPerDay;
        }

        public void Start()
        {
            if (_runningTask != null)
            {
                throw new InvalidOperationException("任务已经在运行.");
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _runningTask = RunPeriodicTaskAsync(_cancellationTokenSource.Token);
            return ;
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

        private async Task RunPeriodicTaskAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 获取到下一次任务的开始延迟时间
                    var nextRunDelay = GetNextStartDelay();
                    Console.WriteLine($"等待到下次任务开始时间: {nextRunDelay}");
                    await Task.Delay(nextRunDelay, cancellationToken);

                    // 在指定的天数里，按照设定的次数和间隔运行任务
                    for (int i = 0; i < _executionCountPerDay; i++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        Console.WriteLine($"第 {i + 1} 次任务执行");
                        await _taskToRun();

                        if (i < _executionCountPerDay - 1)  // 除了最后一次，等待下一个任务的间隔时间
                        {
                            Console.WriteLine($"等待下一个任务执行的间隔: {_executionInterval}");
                            await Task.Delay(_executionInterval, cancellationToken);
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发生异常: {ex.Message}");
                }
            }
        }

        private TimeSpan GetNextStartDelay()
        {
            var now = DateTime.Now;

            // 获取今天是星期几
            var currentDayOfWeek = now.DayOfWeek;

            // 计算下一次任务执行的时间
            DateTime nextStartTime = DateTime.MinValue;

            foreach (var dayOfWeek in _daysOfWeekToRun.OrderBy(d => (int)d))
            {
                // 计算本周或下周的运行时间
                var nextRunTime = new DateTime(now.Year, now.Month, now.Day,
                                               _dailyStartTime.Hours, _dailyStartTime.Minutes, _dailyStartTime.Seconds);

                // 如果是当天，且当前时间已经过了任务开始时间，计算到下一周的这个时间
                if (currentDayOfWeek == dayOfWeek && now.TimeOfDay > _dailyStartTime)
                {
                    nextRunTime = nextRunTime.AddDays(7);  // 跳到下周
                }
                else if (currentDayOfWeek > dayOfWeek) // 如果当前时间已经过了设定的执行天数，计算下周
                {
                    nextRunTime = nextRunTime.AddDays((int)dayOfWeek - (int)currentDayOfWeek + 7);
                }
                else if (currentDayOfWeek < dayOfWeek) // 如果还没到设定的执行天数，计算本周的剩余天数
                {
                    nextRunTime = nextRunTime.AddDays(dayOfWeek - currentDayOfWeek);
                }

                // 选出最近的一次执行时间
                if (nextStartTime == DateTime.MinValue || nextRunTime < nextStartTime)
                {
                    nextStartTime = nextRunTime;
                }
            }

            return nextStartTime - now;
        }
    }
}
