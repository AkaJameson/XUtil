namespace DotNet.Util.Core.IniParser
{
    public interface IReadOnlyKeyValuePairs
    {
        string this[string key] { get; }    

        IEnumerable<string> Keys { get; }
        IEnumerable<string> Values { get; }
    }
}
