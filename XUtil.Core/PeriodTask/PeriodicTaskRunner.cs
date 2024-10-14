﻿namespace XUtil.Core.PeriodTask
{
    public class PeriodicTaskRunner:IPeriodTask
    {
        private readonly Func<Task> _taskToRun;
        private readonly TimeSpan _timeout;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _runningTask;

        public PeriodicTaskRunner(Func<Task> taskToRun, TimeSpan timeout)
        {
            _taskToRun = taskToRun ?? throw new ArgumentNullException(nameof(taskToRun));
            _timeout = timeout;
        }

        public void Start()
        {
            if (_runningTask != null)
            {
                throw new InvalidOperationException("The periodic task is already running.");
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _runningTask = RunPeriodicTaskAsync(_cancellationTokenSource.Token);
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

        private async Task RunPeriodicTaskAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _taskToRun();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"未知异常: {ex.Message}");
                }

                try
                {
                    await Task.Delay(_timeout, cancellationToken);
                }
                catch (TaskCanceledException)
                {

                    break;
                }
            }
        }
    }
}
