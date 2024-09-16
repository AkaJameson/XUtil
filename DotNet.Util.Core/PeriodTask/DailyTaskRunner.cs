using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.PeriodTask
{
    public class DailyTaskRunner
    {
        private readonly Func<Task> _taskToRun;
        private readonly TimeSpan _dailyRunTime;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _runningTask;

        public DailyTaskRunner(Func<Task> taskToRun, TimeSpan dailyRunTime)
        {
            _taskToRun = taskToRun ?? throw new ArgumentNullException(nameof(taskToRun));
            _dailyRunTime = dailyRunTime;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public DailyTaskRunner Start()
        {
            if (_runningTask != null)
            {
                throw new InvalidOperationException("已经运行.");
            }

            _runningTask = RunDailyTaskAsync(_cancellationTokenSource.Token);
            return this;
        }

        public DailyTaskRunner Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _runningTask = null;
            }
            return this;
        }

        private async Task RunDailyTaskAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var nextRunDelay = GetNextRunDelay();
                    Console.WriteLine($"下次运行时间: {nextRunDelay}");
                    await _taskToRun();
                    await Task.Delay(nextRunDelay, cancellationToken);
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
            // Get current time
            var now = DateTime.Now;

            // Calculate the next run time
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day,
                                           _dailyRunTime.Hours, _dailyRunTime.Minutes, _dailyRunTime.Seconds);

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            return nextRunTime - now;
        }
    }
}
