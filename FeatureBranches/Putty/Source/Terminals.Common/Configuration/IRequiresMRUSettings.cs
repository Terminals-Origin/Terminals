namespace Terminals.Configuration
{
    /// <summary>
    /// Provider of controls, which require most recently used store.
    /// </summary>
    public interface IRequiresMRUSettings
    {
        IMRUSettings Settings { get; set; }
    }
}