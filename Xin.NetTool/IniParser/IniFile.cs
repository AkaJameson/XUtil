using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.NetTool.IniParser
{
    /// <summary>
    /// 默认支持Utf-8
    /// </summary>
    public class IniFile:IDisposable
    {
        private string filePath;
        private Dictionary<string,Dictionary<string,string>> iniDictonary;
        private bool isEdit = false;
        public IniFile(string filePath)
        {
            iniDictonary = new();
            this.filePath = filePath;
            LoadIni();
        }

        public void LoadIni()
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("未找到这个文件");
            }

            //检查这个文件的后缀名是不是ini
            if (!Path.GetExtension(filePath).Equals(".ini"))
            {
                throw new Exception("这个文件不是ini配置文件");
            }
            List<string> lines = File.ReadAllLines(filePath).Where(x=>!string.IsNullOrEmpty(x)&&!x.StartsWith(";")).ToList();
            lines.ForEach(x => x.Trim());
            string section="default";
            foreach(var line in lines)
            {
                if(line.StartsWith("[")&& line.EndsWith("]"))
                {
                    section = line.Substring(1, line.Length - 2);
                    if (!iniDictonary.ContainsKey(section))
                    {
                        Dictionary<string, string> keyValuePairs = new();
                        iniDictonary.Add(section, keyValuePairs);
                    }
                    continue;
                }
                string[] keyvaluepair = line.Split(new[] { '=' });
               
                if (section.Equals("default"))
                {
                    throw new Exception("当前ini文件的格式不正确");
                }
                iniDictonary[section].Add(keyvaluepair[0].Trim(), keyvaluepair[1].Trim());
            }

        }

        /// <summary>
        /// 读取value
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadValue(string section,string key)
        {
            if (iniDictonary.ContainsKey(section))
            {
                if (iniDictonary[section].ContainsKey(key))
                {
                    return iniDictonary[section][key];
                }
            }
            return null;
        }
        /// <summary>
        /// 读取节点KeyValue值
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Dictionary<string,string> ReadKeyValuePairsInSection(string section)
        {
            return iniDictonary[section];
        }
        /// <summary>
        /// 修改值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool EditValue(string section,string key,string value)
        {
            if (iniDictonary.ContainsKey(section))
            {
                if (iniDictonary[section].ContainsKey(key))
                {
                    iniDictonary[section][key]=value;
                    isEdit = true;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="oldSection"></param>
        /// <param name="newSection"></param>
        /// <returns></returns>
        public bool EditSection(string oldSection,string newSection)
        {
            if (iniDictonary.ContainsKey(oldSection))
            {
                Dictionary<string, string> keyValuePairs = iniDictonary[oldSection];
                iniDictonary.Remove(oldSection);
                iniDictonary.Add(newSection, keyValuePairs);
                isEdit = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 修改Key
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        public bool EditKey(string Section,string oldKey,string newKey)
        {
            if (iniDictonary.ContainsKey(Section) && iniDictonary[Section].ContainsKey(oldKey))
            {
                string value = iniDictonary[Section][oldKey];
                iniDictonary[Section].Remove(oldKey);
                iniDictonary[Section].Add(newKey,value);
                isEdit = true;
            }
            return false;
        }
        public bool AddSection(string section)
        {
            if (!iniDictonary.ContainsKey(section))
            {
                Dictionary<string, string> keyValuePairs = new();
                iniDictonary.Add(section, keyValuePairs);
                isEdit = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加键值对到指定的节点，节点不存在会主动创建
        /// </summary>
        /// <returns></returns>
        public void AddKeyValueInSection(string section,string key,string value)
        {
            if (!iniDictonary.ContainsKey(section))
            {
                AddSection(section);
                iniDictonary[section].Add(key, value);
            }
            else
            {
                iniDictonary[section].Add(key, value);
            }
        }
        /// <summary>
        /// 这个功能会覆盖原来的文件（注释会丢）
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            if (isEdit)
            {
                using (StreamWriter sw = new StreamWriter(filePath,false))
                {
                    foreach(var dict in iniDictonary.Keys)
                    {
                        sw.WriteLine($"[{dict}]");
                        foreach(var keyvalue in iniDictonary[dict])
                        {
                            sw.WriteLine($"{keyvalue.Key} = {keyvalue.Value}");
                        }
                        sw.WriteLine();
                        sw.WriteLine();
                    }
                }
            }
            else
            {
                throw new Exception("这个ini文件没有被修改过");
            }
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Reload()
        {
            iniDictonary.Clear();
            List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith(";")).ToList();
            lines.ForEach(x => x.Trim());
            string section = "default";
            foreach (var line in lines)
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line.Substring(1, line.Length - 2);
                    if (!iniDictonary.ContainsKey(section))
                    {
                        Dictionary<string, string> keyValuePairs = new();
                        iniDictonary.Add(section, keyValuePairs);
                    }
                    continue;
                }
                string[] keyvaluepair = line.Split(new[] { '=' });

                if (section.Equals("default"))
                {
                    throw new Exception("当前ini文件的格式不正确");
                }
                iniDictonary[section].Add(keyvaluepair[0].Trim(), keyvaluepair[1].Trim());
            }

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool b)
        {
            if (b)
            {
                iniDictonary = null;
            }
        }
    }
}
