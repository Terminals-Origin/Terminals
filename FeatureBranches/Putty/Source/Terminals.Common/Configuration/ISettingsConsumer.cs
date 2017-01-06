namespace Terminals.Configuration
{
    public interface ISettingsConsumer
    {
        IConnectionSettings Settings { get; set; }
    }
}