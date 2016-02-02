namespace Terminals.Configuration
{
    internal interface ISettingsConsumer
    {
        IConnectionSettings Settings { get; set; }
    }
}