namespace Terminals.Common.Connections
{
    /// <summary>
    /// Extension of plugins supporting conversions between data models V1 and V2.
    /// </summary>
    public interface IOptionsConverterFactory
    {
        IOptionsConverter CreatOptionsConverter();
    }
}