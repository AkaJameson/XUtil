## DotNet.Util.Core

### SnowFlake（雪花算法）

为了解决Guid 128位过长占据数据库的存储空间，并且无法再分布式系统中使用的原因。

使用c#重写了Twitter的雪花算法,线程安全，并且适合于分布式系统的使用（正确设置工区Id和作业Id）

```c#
using Xin.DotnetUtil.SnowFlake
    
SnowflakeIdGenerator generator = new SnowflakeIdGenerator(workerId,datacenterId,timeCallBackHandler);
string id = generator.nextId();
//timeCallBackHandler 是对时间回溯异常的处理委托
```

### JobManager（作业模式）

  仅适用于windows平台
  作业对象，主要用于子进程管理。
  目前版本是只支持作业销毁拥有的子进程退出,仅支持了一个Job对象，需要使用可以修改Job句柄为可变参数
  通常可以定义一个全局静态变量使用

```c#
using  Xin.DotnetUtil.JobManager

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

### EventBus（事件总线）

事件总线是对发布-订阅模式的一种实现。它是一种集中式事件处理机制，允许不同的组件之间进行彼此通信而又不需要相互依赖，达到一种解耦的目的。

不依赖于高耦合的delegate和Event,实现了一个线程安全类型的事件总线形式。

```c#
using  Xin.DotnetUtil.EventBus

//组件已经实现了自动注册功能
//发布，及触发
EventBus.Default.publish();
//订阅
EventBus.Default.Subscribe();
//取消订阅
EventBus.Default.UnSubscribe();
```

### EasyLog（简单日志）
一个跨平台的轻量化的简单Log日志，需要创建LogConfig.json。

```c#
using Xin.DotnetUtil.EasyLog
//json格式
{
  "LogFileName": "MYlOG",
  "SuccessLogName": "SuccessLog",
  "ErrorLogName": "ErrorLog",
  "ExpectionLogName": "ExpectionLog",
  "WarningLogName": "WarningLog",
  "SuccessSaveDays": 10,
  "ErrorSaveDays": 20,
  "ExpectionSaveDays": 20,
  "WarningSaveDays": 20

}
/*
* 解释说明：LogFileName：根文件下的Log文件夹
* "Success|Error|Expection|Warning LogName":成功，错误，异常，警告，四种日志文件的名字（不需* * 要后缀名）
* SaveDays：日志会根据设置进行刷新，以日为单位进行刷新
*/

使用
 static void Main(string[] args)
 {
 	// 可以使用绝对路径，也可以使用相对路径，使用相对路径，将LogConfig.json属性设置如果较新则复制
     LogSaver saver = new LogSaver("./LogConfig.json");
     saver.LogWarning("This is a warning log");
     saver.LogWarningAsync("This is a warning log");
 }
```

### IniParser（ini解析器）
Ini文件解析器，提供Ini文件解析，添加，修改等功能。

```c#
using Xin.DotnetUtil.IniFile
IniFile iniFile = new IniFile(path);
//读取指定得Value
iniFile.ReadValue(section,key);
//读取KeyValuePairs
iniFile.ReadKeyValuePairsInSection(section);
//修改节点名字
iniFile.EditSection(oldSection, newSection);
//修改节点下key名字
iniFile.EditKey(Section,oldKey,newKey);
//修改节点下对应Key得Value名字
iniFile.EditValue(section,key,value);
//添加keyValue
iniFile.AddKeyValueInSection("xxxx", "xxxx", "xxxx");
//添加节点
iniFile.AddSection("pilipala");
//重新加载
iniFile.Reload();
//保存 会覆盖源文件，导致注释消失
iniFile.Save();

//支持Idispose释放资源
using(IniFile inif = new IniFile(Path))
{
    //ToDo
}
```

### SysInfo（系统信息）

**SystemInfo和Wintimer仅Windows平台可用**（**跨平台时间计数器可以选择使用NetCore自带StopWatch**）：

```c#
using Xin.DotnetUtil.SysInfo

//纳秒级计时器WinTimer，可以这么写
 WinTimer winTimer = new WinTimer();
 winTimer.Start();
 Thread.Sleep(5000);
 winTimer.Stop();
 Console.WriteLine((string)winTimer);
 Console.WriteLine((double)winTimer);
 Console.WriteLine(winTimer.Duration);
//也可以这么写
using(WinTimer timer = WinTimer.Create())
{
    Thread.Sleep(5000);
    timer.Stop();
    Console.WriteLine(timer.Duration);
}

```

```c#

using Xin.DotnetUtil.SysInfo
//静态类SystemInfo，获取硬件信息（无第三方依赖，仅能使用在windows平台）
 //获取CPU序列号
 public static string GetCpuID();
 //获取MAC地址
 public static string GetMacAddress();
 //获取硬盘ID
 public static string GetHDid();
 //获取Ip地址
 public static string GetIPAddress();
 //获取操作系统类型
 public static string GetOsType();
 //获取主机名
 public static string GetComputerName();
 //获取操作系统架构
 public static string GetOSArchitecture();
 //获取操作系统名称
 public static string GetOSDescription();
 //获取CPU使用率
 public static float GetCPUTotalLoad();
 //获取物理内存总量
 public static long GetTotalMemory();
 //获取已用内存量
 public static long GetUsedMemory();
 //获取可用内存量
 public static long GetAvailableMemory();
```

```c#
using Xin.DotnetUtil.SysInfo
//静态类NetInfo
Task<string> GetPublicIPAsync();
//获取网关和子网掩码
void GetLocalNetworkInfo(out string gateway, out string subnetMask);
//扫描局域网内所有在线设备，获取所有在线设备
List<string> ScanLocalNetwork(string subnet, int timeout = 1000);
//获取子网基地址
string GetSubnet();
```

```c#
 
using Xin.DotnetUtil.SysInfo
// 定时任务池，定时执行任务
 //MyScheduleTest继承ITaskTriggerHaneler实现Occour方法
 //循环任务
 ScheduleTasksPool.SetScheduleTask("helloworld", new MyScheduleTest(), true, 5);
 ScheduleTasksPool.ActiveTask("helloworld");
 //单次执行
  ScheduleTasksPool.SetScheduleTask("helloworld2", new MyScheduleTest2(), false, 1);
  ScheduleTasksPool.ActiveTask("helloworld2");
```

### Securencryption（加解密）

提供AES Base64 DES RSA MD5的加解密验证帮助静态类。

```c#
using Xin.DotnetUtil.Securecryption
//MD5Helper
 string ComputeMd5Hash(string input);//一次加严
 ComputeDoubleMd5Hash(string input);// 二次加严
 VerifyDoubleMd5Hash(string input, string hash);//二次加严验证
 VerifyMd5Hash(string input, string hash);//一次加严验证
     
     
 //Base64Helper
 string EncodeToBase64(string input);//转换成Base64编码
 string DecodeFromBase64(string base64Input);//从Base64解码成UTF8编码
 bool VerifyBase64Encoding(string input, string base64Encoded);//验证base64编码
 bool VerifyBase64Decoding(string base64Input, string decoded);//验证base64解码


 //DESHelper
string Encrypt(string plainText, byte[] key = null, byte[] iv = null);//加密字符串，仅允许8位密钥和8位IV
string Decrypt(string cipherText, byte[] key = null, byte[] iv = null);//解密字符串,仅支持8位密钥和8位IV
bool VerifyEncryption(string plainText, string cipherText, byte[] key = null, byte[] iv = null);//验证给定的明文在加密后是否与提供的密文匹配


//AESHelper
string Encrypt(string plainText, byte[] key = null, byte[] iv = null);//加密字符串
string Decrypt(string cipherText, byte[] key = null, byte[] iv = null);//解密字符串
VerifyEncryption(string plainText, string cipherText, byte[] key = null, byte[] iv = null);//验证给定的明文在加密后是否与提供的密文匹配

//RSAHelper
void GenerateKeys(out string publicKey, out string privateKey);//生成RSA密钥对
string Encrypt(string plainText, string publicKey);//使用RSA公钥加密字符串
string Decrypt(string cipherText, string privateKey);//使用RSA私钥解密字符串
string SignData(string message, string privateKey);//使用 RSA 私钥对字符串进行签名
bool VerifyData(string message, string signature, string publicKey)//使用RSA公钥验证签名
    
//CRC16Modbus 
 byte[] ComputeChecksumBytes(byte[] bytes) //计算CRC校验值
 bool ValidateModbusRTUCRC(byte[] data, byte[] crcBytes)//验证数据是否通过CRC校验
```

### Export（导出）

```c#
Xin.DotnetUtil.Export
//静态类ExportCSV  DATATABLE导出CSV
void ExportToCSV(DataTable dt,string fullpathName);
//CSV导入DataTable
DataTable ImportCSVToDataTable(string fullpathName);

//多线程下载器 支持断点续传和进度保存
//使用方法，通过线程进行演示
Thread thread = new Thread(async () =>
{
    MultiThreadDownloader multiThreadDownloader = new MultiThreadDownloader(断点保存文件的地址！不带文件的名字);
    await multiThreadDownloader.DownloadFilesAsync(new List<string> { "xxxx" },下载到的地址);
    Console.WriteLine("下载完成");
}); thread.start();

//事件回调
/// <summary>
/// 下载完成事件
/// </summary>
Action OnDownloadDoneHandler;
/// <summary>
/// 开始下载事件
/// </summary>
Action OnDownloadStartHandler;
```

### DateTimerHelper(时间计算帮助类)

```c#
using Xin.DotnetUtil.DateTimeHelper
//时间差
double DiffMilliSecond(DateTime start, DateTime end)
double DiffSecound(DateTime start, DateTime end)
double DiffMinute(DateTime start, DateTime end)
double diffHour(DateTime start, DateTime end)
double diffDay(DateTime start, DateTime end)
double diffWeek(DateTime start, DateTime end)
double diffMonth(DateTime start, DateTime end)
double diffYear(DateTime start, DateTime end)
//当前时间计算
 string GetNowStandardDayTime()//获取“yyyy-MM-dd”
 string GetNowStandardTime()//获取当前时间的"HH:mm:ss"
 string GetDateTimeF()//返回当前时间的标准时间格式yyyy-MM-dd HH:mm:ss:fffffff
 string GetUtcTimeTicks()//获取当前时间戳（可以当作唯一标识)
 double ConvertToUnixTimestamp(DateTime date)//转换时间为unix时间戳(date为UTC时间)
//日常判断
 bool IsLeapYear(DateTime dateTime)//是否是闰年
 string DayInWeek(DateTime dateTime,bool isChinese)//今天是周几
 DateTime GetDateWithYearAndDay(int year, int day)//根据年和天数获取日期
 double GetWeekofYear(DateTime dateTime)//获取一年中的第几周
 double GetDayofYear(DateTime dateTime)//获取一年中的第几天
```

### Collection（集合）

提供树形结构创建和BFS先中后序路径查询（非常规BFS，BFS最短路径）和DFS遍历。

```c#
using Xin.DotnetUtil.Collection
//工厂类创建
 ITreeRoot<MyStruct> treeRoot = TreeFactory.CreateRoot(new MyStruct("root"));
 //构造方法创建
 ITreeRoot<MyStruct> treeRoot = new TreeNode<MyStruct>(new MyStruct("root"));
 //工厂类创建添加节点
 var node1 = TreeFactory.CreateNode(new MyStruct("child1"));
 var node2 = TreeFactory.CreateNode(new MyStruct("child1"));
 TreeFactory.AddChild(treeRoot, node1);
 TreeFactory.AddChild(treeRoot, node2);
 //TreeNode 直接添加节点
 treeRoot.AddChild(node1);
 // 工厂类提供BFS前序遍历 中序遍历 后续遍历
 TreeFactory.FindChildDFSPreOrder<MyStruct>(treeRoot, node1.Value, out List<MyStruct> findPath);
 //TreeRoot或者TreeNode提供BFS 前序遍历 中序遍历 后续遍历(从指定节点遍历）
 treeRoot.FindChildDFSInOrder(node2.Value, out List<MyStruct> findPath);
treeRoot.FindChildDFSPostOrder(node2.Value, out List<MyStruct> findPath);
 //DFS通过自定义迭代器提供
 foreach (var item in treeRoot)
 {
     Console.WriteLine(item.name);
 }
```

### Verify(校验工具)

```c#
using Xin.DotnetUtil.Verify
//CRCUtil类
/*
	提供标准Crc校验功能
	 CRC_8，CRC_8_CDMA2000, CRC_8_DARC,  CRC_8_DVB_S2 ,CRC_8_EBU ,CRC_8_I_CODE, CRC_8_ITU, CRC_8_ROHC, CRC_8_MAXIM, CRC_8_WCDMA, CRC_16_ARC, CRC_16_AUG_CCITT, CRC_16_BUYPASS, CRC_16_CCITT_FALSE, CRC_16_CDMA2000, CRC_16_DDS_110 CRC_16_DECT_R, CRC_16_DECT_X,
CRC_16_DNP, CRC_16_EN_13757, CRC_16_GENIBUS, CRC_16_KERMIT, CRC_16_MAXIM, CRC_16_MCRF4XX, CRC_16_MODBUS, CRC_16_RIELLO CRC_16_T10_DIF, CRC_16_TELEDISK, CRC_16_TMS37157, CRC_16_USB,
CRC_16_X25, CRC_16_XMODEM, CRC_A, CRC_32, CRC_32_BZIP2, CRC_32_JAMCRC, CRC_32_MPEG2, CRC_32_POSIX, CRC_32_SATA, CRC_32_XFER, CRC_32C, CRC_32D, CRC_32Q

	提供自定义CRCParameter校验功能
*/
 //示例
     uint Compute(byte[] buffer,CRCCrcAlgorithm cRCCrcAlgorithm, bool littleEndian, int offset=0)
      uint Compute(byte[] buffer, CRCCrcAlgorithm cRCCrcAlgorithm, int offset, bool littleEndian, int count)
     uint Compute(byte[] buffer, CRCParamter standardCRCParamter,bool littleEndian, int offset = 0)
    uint Compute(byte[] buffer, CRCParamter standardCRCParamter, int offset, bool littleEndian,int count)
    byte[] ComputeBytes(byte[] buffer, CRCCrcAlgorithm cRCCrcAlgorithm, bool littleEndian, int offset = 0)
     byte[] ComputeBytes(byte[] buffer, CRCCrcAlgorithm cRCCrcAlgorithm, int offset, int count, bool littleEndian)
    byte[] ComputeBytes(byte[] buffer, CRCParamter standardCRCParamter, bool littleEndian, int offset = 0)
     byte[] ComputeBytes(byte[] buffer, CRCParamter standardCRCParamter, int offset,int count , bool littleEndian)
    
    
    //SimpleVerify类
    //校验和
     byte  CalculateChecksum(byte[] data)
    //异或校验
     byte CalculateXorChecksum(byte[] data)
```

