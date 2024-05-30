## Xin.NetTool

### Xin.SnowFlake

为了解决Guid 128位过长占据数据库的存储空间，并且无法再分布式系统中使用的原因。

使用c#重写了Twitter的雪花算法,线程安全，并且适合于分布式系统的使用（正确设置工区Id和作业Id）

```
SnowflakeIdGenerator generator = new SnowflakeIdGenerator(workerId,datacenterId,timeCallBackHandler);
string id = generator.nextId();
```

### Xin.JobManager

  仅适用于windows平台
  作业对象，主要用于子进程管理。
  目前版本是只支持作业销毁拥有的子进程退出,仅支持了一个Job对象，需要使用可以修改Job句柄为可变参数
  通常可以定义一个全局静态变量使用

```
Job job = new Job();
job.AddProcess(xxxx);
支持使用Using代码块包裹


//环境变量清理功能：实现对主机变量名为Path的内容清理
    /// 1.清除所有无效路径
    /// 2.清除路径文件夹中包含包含某文件（在代码中修改，没有提供修改方法 ）"HHTech.CSM2018.Starter.exe"或"HHTech.CSM2018.Client.exe"
    /// 3.重写path的系统环境变量
EnviornmentCleanr cleaner = new EnviornmentCleanr();
cleaner.ResetkeyPathInEnvironment();
```

### Xin.EventBus

事件总线是对发布-订阅模式的一种实现。它是一种集中式事件处理机制，允许不同的组件之间进行彼此通信而又不需要相互依赖，达到一种解耦的目的。

不依赖于高耦合的delegate和Event,实现了一个线程安全类型的事件总线形式。

```
//组件已经实现了自动注册功能
//发布，及触发
EventBus.Default.publish();
//订阅
EventBus.Default.Subscribe();
//取消订阅
EventBus.Default.UnSubscribe();
```

###Xin.SqlHelper

实现对mysql，oracle，sqlite,mssql数据库操作的一致性。

目的是轻量化。

```

```



Xin.IniParser

Ini文件解析器

