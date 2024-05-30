using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xin.EventBus
{
    /// <summary>
    /// EventData继承IEventData，用于事件处理器的参数
    /// IEventHandler是一个标记接口，用于标记事件处理器
    /// 继承了EventData和IEventHandler接口的类，会被自动注册到事件总线中
    /// </summary>
    public interface IEventHandler
    {

    }
    public interface IEventHandler<EventData> :IEventHandler where EventData:IEventData
    {
        void HandleEvent(EventData eventData);
    }

    public interface IEventData
    {

    }
    /// <summary>
    /// 事件总线模式，通过抽象一个订阅中心，在逻辑上对事件的发布和订阅进行解耦。
    /// </summary>
    public class EventBus
    {
        //饿汉式，线程安全
        private static EventBus eventBus = new EventBus();

        public static EventBus Default
        {
            get
            {
                return eventBus;
            }
        }
        private ConcurrentDictionary<Type,List<Type>> eventDictionary;

        public EventBus()
        {
            eventDictionary = new();
            AutoMapDataToEvent();
        }
        /// <summary>
        /// 自动绑定事件和事件处理器
        /// </summary>

        private void AutoMapDataToEvent()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach(Type type in assembly.GetTypes())
            {
                if(typeof(IEventHandler).IsAssignableFrom(type))
                {
                    Type handleInterface = type.GetInterfaces().Where(x => x.IsGenericType &&
                     x.GetGenericTypeDefinition() == typeof(IEventHandler<>)).FirstOrDefault();

                    if(handleInterface != null)
                    {
                        Type eventdatatype = handleInterface.GetGenericArguments()[0];
                        if(eventDictionary.ContainsKey(eventdatatype))
                        {
                            eventDictionary[eventdatatype].Add(type);
                        }
                        else
                        {
                            eventDictionary[eventdatatype] = new List<Type> { type };
                        }
                    }

                }

            }
        }
        /// <summary>
        /// 手动绑定事件源与事件处理的绑定
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void Subscribe<TEventData>(Type eventHandler)
        {
            if (eventDictionary.ContainsKey(typeof(TEventData)))
            {
                eventDictionary[typeof(TEventData)].Add(eventHandler);
            }
            else
            {
                throw new Exception("Event Handler not found");
            }
        }

        /// <summary>
        /// 手动解除事件源与事件处理的绑定
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void UnSubscribe<TEventData>(Type eventHandler)
        {
            if (eventDictionary.ContainsKey(typeof(TEventData)))
            {
                eventDictionary[typeof(TEventData)].Remove(eventHandler);
            }
            else
            {
                throw new Exception("Event Handler not found");
            }
        }
        /// <summary>
        /// 直接触发事件，不通过委托
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        /// <exception cref="Exception"></exception>
        public void Publish<TEventData>(TEventData eventData)
        {
            if (eventDictionary.ContainsKey(typeof(TEventData)))
            {
                eventDictionary[typeof(TEventData)].ForEach(x =>
                {
                    MethodInfo method = x.GetMethod("HandleEvent");
                    if (method != null)
                    {
                        object eventHandler = Activator.CreateInstance(x);
                        method.Invoke(eventHandler, new object[] { eventData });
                    }
                });
            }
            else
            {
                throw new Exception("Event Handler not found");
            }
        }


    }
        
}
