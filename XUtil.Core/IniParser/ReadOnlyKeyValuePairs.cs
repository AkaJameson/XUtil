using System.Collections.Concurrent;

namespace XUtil.Core.IniParser
{
    public class ReadOnlyKeyValuePairs : IReadOnlyKeyValuePairs
    {
        private readonly ConcurrentDictionary<string, string> _source;

        public ReadOnlyKeyValuePairs(ConcurrentDictionary<string, string> keyValuePairs)
        {
            _source = new ConcurrentDictionary<string, string>(keyValuePairs);
        }

        public string this[string key] => _source[key];

        public IEnumerable<string> Keys => _source.Keys;

        public IEnumerable<string> Values => _source.Values;

        public int Count => _source.Count;

        public bool ContainsKey(string key) => _source.ContainsKey(key);

        public bool TryGetValue(string key, out string value) => _source.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _source.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _source.GetEnumerator();
    }
}
