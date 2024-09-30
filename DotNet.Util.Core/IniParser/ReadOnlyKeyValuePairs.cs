using System.Collections.Concurrent;

namespace DotNet.Util.Core.IniParser
{
    public class ReadOnlyKeyValuePairs : IReadOnlyKeyValuePairs
    {
        private readonly ConcurrentDictionary<string, string> _source;
        public ReadOnlyKeyValuePairs(ConcurrentDictionary<string,string> keyValuePairs)
        {
            _source = keyValuePairs;
        }
        public string this[string key] => _source[key];
        public IEnumerable<string> Keys => _source.Keys;
        public IEnumerable<string> Values => _source.Values;
    }
}
