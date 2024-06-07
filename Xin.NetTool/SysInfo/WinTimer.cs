using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.SysInfo
{
    /// <summary>
    /// 仅适用于windows,纯计时器
    /// </summary>
    public class WinTimer : IDisposable
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
         out long lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
          out long lpFrequency);
        private bool isCount;
        private long _startTime, _stopTime;
        private long _freq;

        public WinTimer()
        {
            _startTime = 0;
            _stopTime = 0;
            isCount = false;
            //是否有触发时间
            if (QueryPerformanceFrequency(out _freq) == false)
            {
                throw new Exception("该操作系统不支持WinTimer");
            }
        }
        public double Duration
        {
            get
            {
                return (double)(_stopTime - _startTime) / (double)_freq;
            }
        }
        public static implicit operator double(WinTimer timer)
        {
            return timer.Duration;
        }
        public static implicit operator string(WinTimer timer)
        {
            return string.Format("{0:F}", timer.Duration);
        }
        public void Start()
        {
            isCount = true;
            QueryPerformanceCounter(out _startTime);
        }

        public void Stop()
        {
            if (isCount)
            {
                isCount = false;
                QueryPerformanceCounter(out _stopTime);
            }
            else
            {
                throw new Exception("计时器还没有启动或者已经暂停");
            }
        }

        public void Continue()
        {
            isCount = true;
        }
        

        public static WinTimer Create()
        {
            WinTimer timer =  new WinTimer();
            timer.Start();
            return timer;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
   
}
