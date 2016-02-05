using SSHClient;

namespace Terminals.Configuration
{
    internal interface IConnectionSettings
    {
        bool AskToReconnect { get; set; }

        KeysSection SSHKeys { get; }
    }
}