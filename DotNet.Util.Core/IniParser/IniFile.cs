using System.Collections.Concurrent;

namespace Xin.DotnetUtil.IniParser
{
    /// <summary>
    /// 默认支持Utf-8  目前只是个玩具
    /// </summary>
    public class IniFile
    {
        private string filePath;
        private ConcurrentDictionary<string,ConcurrentDictionary<string,string>> iniDictonary;
        private static object _lock = new object();
        private bool isEdit = false;
        private bool isLoad = false;
        public IniFile(string filePath)
        {
            iniDictonary = new();
            this.filePath = filePath;
            LoadIni();
        }

        public void LoadIni()
        {
            if (isLoad)
                return;
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
                        ConcurrentDictionary<string, string> keyValuePairs = new();
                        iniDictonary[section] = keyValuePairs;
                    }
                    continue;
                }
                string[] keyvaluepair = line.Split(new[] { '=' });
               
                if (section.Equals("default"))
                {
                    throw new Exception("当前ini文件的格式不正确");
                }
                iniDictonary[section][keyvaluepair[0].Trim()]= keyvaluepair[1].Trim();
            }
            isLoad = true;

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
        public ConcurrentDictionary<string,string> ReadKeyValuePairsInSection(string section)
        {
            iniDictonary.TryGetValue(section, out var keyValuePairs);
            return keyValuePairs;
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
                ConcurrentDictionary<string, string> keyValuePairs = iniDictonary[oldSection];
                iniDictonary.Remove(oldSection,out ConcurrentDictionary<string,string> result);
                iniDictonary[newSection] = keyValuePairs;
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
                iniDictonary[Section].Remove(oldKey,out string result);
                iniDictonary[Section][newKey]=value;
                isEdit = true;
            }
            return false;
        }
        public bool AddSection(string section)
        {
            if (!iniDictonary.ContainsKey(section))
            {
                ConcurrentDictionary<string, string> keyValuePairs = new();
                iniDictonary[section]=keyValuePairs;
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
                iniDictonary[section][key] = value;
                isEdit = true;
            }
            else
            {
                iniDictonary[section][key]= value;
                isEdit = true;
            }
        }
        /// <summary>
        /// 这个功能会覆盖原来的文件（注释会丢）
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            lock (_lock)
            {
                if (isEdit)
                {
                    using (StreamWriter sw = new StreamWriter(filePath, false))
                    {
                        foreach (var dict in iniDictonary.Keys)
                        {
                            sw.WriteLine($"[{dict}]");
                            foreach (var keyvalue in iniDictonary[dict])
                            {
                                sw.WriteLine($"{keyvalue.Key} = {keyvalue.Value}");
                            }
                            sw.WriteLine();
                            sw.WriteLine();
                        }
                    }
                    isEdit = false;
                }
                else
                {
                    throw new Exception("这个ini文件没有被修改过");
                }
            }
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Reload()
        {
            lock (_lock)
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
                            ConcurrentDictionary<string, string> keyValuePairs = new();
                            iniDictonary[section] = keyValuePairs;
                        }
                        continue;
                    }
                    string[] keyvaluepair = line.Split(new[] { '=' });

                    if (section.Equals("default"))
                    {
                        throw new Exception("当前ini文件的格式不正确");
                    }
                    iniDictonary[section][keyvaluepair[0].Trim()] = keyvaluepair[1].Trim();

                }
                isLoad = true;

            }
        }
    }
}
