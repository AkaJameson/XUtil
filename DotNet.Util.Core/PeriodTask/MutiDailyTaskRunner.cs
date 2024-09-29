namespace Xin.DotnetUtil.PeriodTask
{
    public class MutiDailyTaskRunner:IPeriodTask
    {
        private readonly Func<Task> _taskToRun;
        private readonly TimeSpan _dailyStartTime;  // 每天开始执行任务的时间
        private readonly TimeSpan _executionInterval;  // 执行任务的间隔
        private readonly int _executionCountPerDay;  // 每天执行的次数
        private CancellationTokenSource _cancellationTokenSource;
        private Task _runningTask;

        public MutiDailyTaskRunner(Func<Task> taskToRun, TimeSpan dailyStartTime, TimeSpan executionInterval, int executionCountPerDay)
        {
            _taskToRun = taskToRun ?? throw new ArgumentNullException(nameof(taskToRun));
            _dailyStartTime = dailyStartTime;
            _executionInterval = executionInterval;
            _executionCountPerDay = executionCountPerDay;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public MutiDailyTaskRunner Start()
        {
            if (_runningTask != null)
            {
                throw new InvalidOperationException("任务已经在运行.");
            }

            _runningTask = RunPeriodicTaskAsync(_cancellationTokenSource.Token);
            return this;
        }

        public MutiDailyTaskRunner Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _runningTask = null;
            }
            return this;
        }

        private async Task RunPeriodicTaskAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 等待到每天的开始时间
                    var nextRunDelay = GetNextStartDelay();
                    Console.WriteLine($"等待到任务开始时间: {nextRunDelay}");
                    await Task.Delay(nextRunDelay, cancellationToken);

                    // 开始执行任务
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
            // 当前时间
            var now = DateTime.Now;

            // 计算当天任务的开始时间
            var nextStartTime = new DateTime(now.Year, now.Month, now.Day,
                                             _dailyStartTime.Hours, _dailyStartTime.Minutes, _dailyStartTime.Seconds);

            if (now > nextStartTime)
            {
                // 如果当前时间已经过了开始时间，那么等待到明天
                nextStartTime = nextStartTime.AddDays(1);
            }

            return nextStartTime - now;
        }
    }
}
