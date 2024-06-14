using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.SysInfo
{
    /// <summary>
    /// 定时任务池
    /// </summary>
    public static class ScheduleTasksPool
    {
        static ConcurrentDictionary<string, ScheduleTask> tasks = new ConcurrentDictionary<string, ScheduleTask>();

        //设置定时任务
        public static void SetScheduleTask(string taskName, ITaskTriggerHaneler taskTriggerHaneler, bool isLoop, double triggerTime)
        {
            if (isLoop)
            {
                Thread thread = new(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(triggerTime));
                        try
                        {
                            taskTriggerHaneler.Occur();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception in task '{taskName}': {ex.Message}");
                            break;
                        }
                    }
                });
                //加入定时任务池
                ScheduleTask scheduleTask = new(taskName, thread, triggerTime);
                bool b = tasks.TryAdd(taskName, scheduleTask);
                if (!b)
                {
                    throw new Exception("发生未知异常");
                }
            }
            else
            {
                Thread thread = new(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(triggerTime));
                    try
                    {
                        taskTriggerHaneler.Occur();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception in task '{taskName}': {ex.Message}");
                    }
                });
                //加入任务池
                ScheduleTask scheduleTask = new(taskName, thread, triggerTime, false);
                bool b = tasks.TryAdd(taskName, scheduleTask);
                if (!b)
                {
                    throw new Exception("发生未知异常");
                }
            }
        }

        //激活任务
        public static void ActiveTask(string taskName)
        {
            if (tasks.TryGetValue(taskName, out var task))
            {
                task.Start();
            }
            else
            {
                throw new Exception("任务不存在");
            }
        }
        //暂停任务
        public static void PauseTask(string taskName)
        {
            if (tasks.TryGetValue(taskName, out var task))
            {
                task.Stop();
            }
            else
            {
                throw new Exception("任务不存在");
            }
        }
        //获取任务
        public static ScheduleTask GetScheduleTask(string taskName)
        {
            if (tasks.TryGetValue(taskName, out var task))
            {
                return task;
            }
            else
            {
                throw new Exception("任务不存在");
            }

        }
        //清除任务
        public static void ClearTask(string taskName)
        {
            if (tasks.TryGetValue(taskName, out var task))
            {
                task.Stop();
                tasks.TryRemove(taskName, out _);
            }
            else
            {
                throw new Exception("任务不存在");
            }
        }
        //清除所有任务
        public static void ClearAllTask()
        {
            foreach (var item in tasks)
            {
                item.Value.Stop();
            }
            tasks.Clear();
        }
    }

    public class ScheduleTask
    {
        public string TaskName { get; set; }

        private Thread thread;
        public bool isLoop { get; set; }
        public double loopTime { get; set; }

        public Action<ITaskTriggerHaneler> Haneler { get; set; }
        public double triggerTime { get; set; }
        //是否在运行中（非循环是否被触发）
        public bool isAlive { get
            {
                return thread.IsAlive;
            } }


        //循环触发
        public ScheduleTask(string TaskName,Thread thread,double loopTime)
        {
            this.TaskName = TaskName;
            this.isLoop = true;
            this.thread = thread;
            this.loopTime = loopTime;
            thread.IsBackground = true;
        }
        //定时触发
        public ScheduleTask(string TaskName, Thread thread,double triggerTime,bool isLoop=false)
        {
            this.TaskName = TaskName;
            this.isLoop = false;
            this.thread = thread;
            this.triggerTime = triggerTime;
            thread.IsBackground = true;
        }

        /// <summary>
        /// start the task
        /// </summary>
        public void Start()
        {
            thread.Start();
        }
        /// <summary>
        /// stop the task
        /// </summary>
        public void Stop()
        {
            thread.Abort();
        }


    }
    public interface ITaskTriggerHaneler
    {
        void Occur();
    }
}
