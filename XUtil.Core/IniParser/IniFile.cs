using System.Collections.Concurrent;
using System.Text;

namespace XUtil.Core.IniParser
{
    /// <summary>
    /// ini文件解析器
    /// </summary>
    public class IniFile
    {
        private string filePath;
        private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> iniDictonary;
        private LinkedList<string> iniList;
        private Encoding fileEncoding;
        private static object _lock = new object();
        private bool isEdit = false;
        private bool isLoad = false;
        public IniFile(string filePath,Encoding encoding)
        {
            iniDictonary = new();
            iniList = new LinkedList<string>();
            this.filePath = filePath;
            fileEncoding = encoding;
            LoadIni();
        }

        private void LoadIni()
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
            var alllines = File.ReadAllLines(filePath,fileEncoding);
            foreach(var t in alllines)
            {
                iniList.AddLast(t);
            }
            var lines = alllines.Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith(";") && !x.StartsWith("#"))
            .Select(x => x.Trim())
            .ToList();
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
                string[] keyvaluepair = line.Split(new[] { '=' },2);
                
                if (section.Equals("default") || keyvaluepair.Length !=2)
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
        /// 将节点和数据写成这样的形式：section:key
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ReadValue(string sec_key)
        {
            var format = sec_key.Split(':');
            if (format.Length != 2)
                return null;
            if (iniDictonary.TryGetValue(format[0], out var sectionDict))
            {
                // 再检查 key 是否存在
                if (sectionDict.TryGetValue(format[1], out var value))
                {
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 读取节点KeyValue值,并不会影响原始节点的数据
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public IReadOnlyKeyValuePairs ReadKeyValuePairsInSection(string section)
        {
            if(iniDictonary.TryGetValue(section, out var keyValuePairs))
            {
                IReadOnlyKeyValuePairs readOnlyKeyValuePairs = new ReadOnlyKeyValuePairs(keyValuePairs);
                return readOnlyKeyValuePairs;
            }
            return null;
        }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private bool AddSection(string section)
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
                lock (_lock)
                {
                    iniList.AddLast(section);
                    iniList.AddLast($"{key}={value}");
                }
                isEdit = true;

            }
            else
            {
                iniDictonary[section][key] = value;
                lock (_lock)
                {
                    var sectionNode = iniList.Find($"[{section}]");
                    bool keyHasFound = false;
                    if (sectionNode != null)
                    {
                        var currentNode = sectionNode.Next;
                        while (currentNode != null && !currentNode.Value.StartsWith("["))
                        {
                            string[] keyValue = currentNode.Value.Split(new[] { '=' }, 2);
                            if (keyValue.Length == 2 && keyValue[0].Trim() == key)
                            {
                                // 如果找到相同的键，更新其值
                                currentNode.Value = $"{key} = {value}";
                                keyHasFound = true;
                                break;
                            }
                            currentNode = currentNode.Next;
                        }
                        if (!keyHasFound && currentNode == null)
                        {
                            iniList.AddLast($"{key} = {value}");
                        }
                        else if(!keyHasFound && currentNode.Value.StartsWith("["))
                        {
                            currentNode.Previous.Value = $"{key} = {value}";
                        }
                    }
                }
                isEdit = true;
            
            }
        }
        /// <summary>
        /// 添加节点， 将节点和数据写成这样的形式：section:key:value
        /// </summary>
        /// <param name="sec_key_value"></param>
        public void AddKeyValueInSection(string sec_key_value)
        {
            var format = sec_key_value.Split(":");
            if (format.Length != 3)
                return;
            AddKeyValueInSection(format[0], format[1],format[2]);
        }
        /// <summary>
        /// 这个功能会覆盖原来的文件
        /// </summary>
        /// <returns></returns>
        public IniFile Save()
        {
            lock (_lock)
            {
                if (isEdit)
                {
                    File.WriteAllLines(filePath, iniList.ToArray(),fileEncoding);
                    isEdit = false;
                }
            }
            return this;
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
                iniList.Clear();
                var alllines = File.ReadAllLines(filePath,fileEncoding);
                foreach (var t in alllines)
                {
                    iniList.AddLast(t);
                }
                var lines = alllines.ToList().Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith(";") && !x.StartsWith("#")).Select(x => x.Trim())
                .ToList();
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
                    string[] keyvaluepair = line.Split(new[] { '=' },2);

                    if (section.Equals("default") || keyvaluepair.Length != 2)
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
