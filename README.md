## DotNetUtil

### EasyTCP

TCP 服务端和客户端封装。

解决TCP底层粘包，双端提供保活，客户端断线重连。

TcpServer

```c#
//创建及启动
TCPServer server = new TCPServer(new XUtil.Core.EasyTcp.Model.SocketConfig());
//停止监听，清理server维护client列表
server.Stop();
//重新启动
server.Start();
//获取服务端工具包
var toolkit = server.GetServerToolkit();
//单点发送消息
toolkit.SendMsg(byte[] msg, string Ipaddress);
//广播
tookit.Boradcast(List<byte[] msg>)
//获取Client模型
toolkit.GetClientModel(string Ipaddress)；
//添加监听到数据处理方法 byte[]:数据,string:RemoteIpAddress
toolkit.AddMessageHandler(Action<byte[], string> handler)
```

TCPClientBase

```c#
//创建及启动
TCPClientBase client = new  TCPClientBase(IPEndPoint iPEndPoint, List<Action<byte[]>> msgHandler = null);
//获取工具包
var toolkit = server.GetClientToolKit();
//工具包提供方法
//发送消息
 Task SendMsg(byte[] msg);
//获取模型
 TCPClientModel GetTcpClient();
//添加消息处理
void AddMessageHandler(Action<byte[]> handler);
```

### EasyUDP

//As Server

```c#
class Program
{
    static void Main(string[] args)
    {
        UdpHelper udpServer = new UdpHelper(8080); // 监听本地8080端口
        udpServer.StartListeningAsync((message, endPoint) =>
        {
            //todo
        });

        Console.WriteLine("UDP Server is listening on port 8080...");
        Console.ReadLine();

        udpServer.Close(); // 停止服务器
    }
}

```

// As Client

```c#
class Program
{
    static void Main(string[] args)
    {
        UdpHelper udpClient = new UdpHelper("127.0.0.1", 8080); // 连接到服务器127.0.0.1的8080端口
        udpClient.Send("Hello, Server!");
 	    udpClient.StartListeningAsync((message, endPoint) =>
        {
            //todo
        });
        Console.WriteLine("Message sent to server.");
        udpClient.Close(); // 关闭客户端
    }
}
```



### SnowFlake（雪花算法）

为了解决Guid 128位过长占据数据库的存储空间，并且无法再分布式系统中使用的原因。

使用c#重写了Twitter的雪花算法,线程安全，并且适合于分布式系统的使用（正确设置工区Id和作业Id）

```c#
using XUtil.Core.SnowFlake
    
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
using  XUtil.Core.JobManager

Job job = new Job();
job.AddProcess(xxxx);
支持使用Using代码块包裹

//环境变量清理功能：实现对主机变量名为Path的内容清理
    /// 1.清除所有无效路径
    /// 2.清除路径文件夹中包含包含某文件
    /// 3.重写path的系统环境变量
EnviornmentCleanr cleaner = new EnviornmentCleanr();
cleaner.ResetkeyPathInEnvironment();
```

### EventBus（事件总线）

事件总线是对发布-订阅模式的一种实现。它是一种集中式事件处理机制，允许不同的组件之间进行彼此通信而又不需要相互依赖，达到一种解耦的目的。

不依赖于高耦合的delegate和Event,实现了一个线程安全类型的事件总线形式。

```c#
using  XUtil.Core.EventBus

//组件已经实现了自动注册功能
//发布，及触发
EventBus.Default.publish();
//订阅
EventBus.Default.Subscribe();
//取消订阅
EventBus.Default.UnSubscribe();
```

### EasyLog（简单日志）
一个轻量化的日志系统，支持多线程。

```c#
using XUtil.Core.Log
    //首先创建LogConfig 提供默认属性
    var logConfig = new LogConfig();
	//使用工厂创建单例
	var logger = LogFactory.CreateLogger(logConfig);
	logger.Info(msg);
	logger.warn(msg);
	log.Error(msg);
```

### IniParser（ini解析器）
线程安全的ini解析修改器，支持 # ；开头注释

```c#
using XUtil.Core.IniParser
    //创建IniFile
    var iniFile = new IniFile(filePath,encoding);
	string ReadValue(string section,string key);
	string ReadValue(string sec_key);
    IReadOnlyKeyValuePairs ReadKeyValuePairsInSection(string section);
	//此方法支持添加修改，不支持删除配置。
    void AddKeyValueInSection(string section,string key,string value);
	void AddKeyValueInSection(string sec_key_value);
	IniFile Save();
	//Save后需要调用Reload进行重新加载；
 	void Reload();
```

### SysInfo（系统信息）

**SystemInfo和Wintimer仅Windows平台可用**（**跨平台时间计数器可以选择使用NetCore自带StopWatch**）：

```c#
using XUtil.Core.SysInfo

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

using XUtil.Core.SysInfo
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
using XUtil.Core.SysInfo
//静态类NetInfo
Task<string> GetPublicIPAsync();
//获取网关和子网掩码
void GetLocalNetworkInfo(out string gateway, out string subnetMask);
//扫描局域网内所有在线设备，获取所有在线设备
List<string> ScanLocalNetwork(string subnet, int timeout = 1000);
//获取子网基地址
string GetSubnet();
```

### Securencryption（加解密）

提供AES Base64 DES RSA MD5,Sha256 sha512的加解密验证帮助静态类。

```c#
using XUtil.Core.Securecryption
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
XUtil.Core.Export
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
using XUtil.Core.DateTimeHelper
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
using XUtil.Core.Collection
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
using XUtil.Core.Verify
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



### RegexHelper(正则校验)

基于网上常见的实现。

```c#
using XUtil.Core
/// <summary>
/// 获取已经匹配到的集合
/// </summary>
/// <param name="input"></param>
/// <param name="pattern"></param>
/// <returns></returns>
public static MatchCollection FindMatches(string input, string pattern)
//常用正则判断
 /// 判断是否是Email地址
 public static bool IsEmail(string input)
 /// 判断域名
 public static bool IsDomain(string input)
 ///判断URL
 public static bool IsUrl(string input)
 /// 判断手机号码
 public static bool IsPhoneNumber(string input)
 ///判断电话号码 
 public static bool IsTelephoneNumber(string input)
 /// 国内电话号码(0511-4405222、021-87888822)
 public static bool IsChineseTelephoneNumber(string input)
 ///身份证号
 public static bool IsIdCardNumber(string input)
 // 密码，账号，数字，字符，网络等等 都在RegexHelper类中
```

### HttpHelper

httpClient封装

```c#
using XUtil.Core
    //创建全局单一持有的http对象请求
    HttpCreateFactory.CreateInstanceRequest();
	//创建请求对象
	HttpCreateFactory.CreateRequest();
//IRequest 方法
 Task<T> GetAsync<T>(string url) where T:class;

 T Get<T> (string url) where T: class;

Task<T> GetAsync<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header) where T : class;

T Get<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header) where T : class;

Task<string> GetStringAsync(string url);

string GetString(string url);

Task<string> GetStringAsync(string url,IDictionary<string,string>? parameters,IDictionary<string, string>? header);

string GetString(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header);

Task<T> PostAsync<T>(string url) where T : class;

T Post<T>(string url) where T : class;

Task<T> PostAsync<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header,object? body) where T : class;

T Post<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header,object? body) where T : class;

Task<string> PostStringAsync(string url);

string PostString(string url);

Task<string> PostStringAsync(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header, object? body);

string PostString(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? header,object? body);

public void SetBaseAddress(string baseAddress);
```



### HttpResponseHelper

统一后端返回格式

```c#
using XUtil.Core.HttpResponseHelper
 public class ApiResponse
 {
     public int Code { get; set; }

     public dynamic Data { get; set; }

     public string? Message { get; set; }

     public string Status { get; set; }

     public int Total { get; set; }

 }
 
 static ApiResponse Success(dynamic data)
 //分页使用
 static ApiResponse Success(dynamic data, int total)
 static ApiResponse Error(string msg)
```

### Extension

```c#
namespace XUtil.Core.Extension
static T ConvertToObject<T>(this string content) where T : class
static string ConvertTostring<T>(this T obj) where T: class
    
namespace XUtil.Core.Extension
StringExtension
bool TrySpilt(string content, char param, out string[] spilts);
TValue StringParse<TValue>(this string value) where TValue : struct
bool IsNotNull(this string value);
```

