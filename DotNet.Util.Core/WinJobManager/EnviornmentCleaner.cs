using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Xin.DotnetUtil.JobManager
{
    /// <summary>
    /// 功能：实现对主机变量名为Path的内容清理
    /// 1.清除所有无效路径
    /// 2.清除路径文件夹中包含包含这个两个文件 "HHTech.CSM2018.Starter.exe"或"HHTech.CSM2018.Client.exe"
    /// 3.重写path的系统环境变量
    /// </summary>
    class EnviornmentCleanr
    {
        /// <summary>
        /// 获取系统变量中的特定变量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetEnvironmentVariable(string key="path")
        {
            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

        }
        /// <summary>
        /// 通过特定的符号分离
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private List<string> SpiltBySpecificSymbols(string source,char key=';')
        {
            List<string> returnString = new List<string>();
            string[] spiltStrings = source.Split(key);
            for (int i = 0; i < spiltStrings.Length; i++)
            {
                returnString.Add(spiltStrings[i]);
            }
            return returnString;
        }
        /// <summary>
        /// 1.通过drive.getDrive()获取到所有的盘符（例如c:\\)保存到list中
        /// 2.根据盘符进行匹配，先把不以盘符开头的path给清除，也就是不保存在cleaned中。
        /// 3.在根据路径文件夹内部有没有包含这个两个文件 "HHTech.CSM2018.Starter.exe"或"HHTech.CSM2018.Client.exe"
        /// </summary>
        /// <param name="preClean"></param>
        /// <returns></returns>
        private List<string> CleanPath(List<string> preClean, string[] reserveFile)
        {
            List<string> cleanedPaths = new List<string>();
            string[] systemPath = { "C:\\Windows\\System32", "C:\\Windows", "C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\WindowsApps" };
            //获取硬盘驱动列表
            var drives = DriveInfo.GetDrives().Select(d => d.Name).ToArray();
            cleanedPaths = preClean.Where(path => drives.Any(drive => path.StartsWith(drive) && !reserveFile.Any(file => File.Exists(Path.Combine(path, file))))).ToList();
            cleanedPaths.AddRange(systemPath.Where(sysPath => !cleanedPaths.Any(path =>path == sysPath)));
            return cleanedPaths;
        }
        /// <summary>
        /// 组合清理过的变脸重新写回到path中
        /// </summary>
        /// <param name="cleanedPath"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ReWriteToEnvironment(List<string> cleanedPath,string key ="path")

        {
            string combine="";
            for(int i = 0; i < cleanedPath.Count - 1;i++)
            {
                combine += cleanedPath[i] + ";";
            }
            combine += cleanedPath[cleanedPath.Count - 1];
            Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable(key, combine, EnvironmentVariableTarget.Machine);
            return combine;

        }
        /// <summary>
        /// 组合上述函数,事务模式
        /// </summary>
        public string ResetkeyPathInEnvironment(string[] reserveFile)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            string cleanpath = "";
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //获取环境变量
                    string wholepath = GetEnvironmentVariable();
                    //分割
                    List<string> splitPath = SpiltBySpecificSymbols(wholepath);
                    //清理
                    List<string> cleanedPath = CleanPath(splitPath, reserveFile);
                    //重新写入
                    cleanpath = ReWriteToEnvironment(cleanedPath);
                    //都执行完成，事务完成
                    ts.Complete();
                }
            }
            catch(TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }
            Console.WriteLine(writer);
            return cleanpath;
          
        }
    }

}
