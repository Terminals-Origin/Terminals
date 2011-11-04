using System;

namespace Terminals.Configuration
{
    internal class ConfigFileChangedEventArgs : EventArgs
    {
        internal ConfigFileChangedEventArgs(TerminalsConfigurationSection oldSettings,
                                            TerminalsConfigurationSection newSettings)
        {
            this.Old = oldSettings;
            this.New = newSettings;
        }

        internal TerminalsConfigurationSection Old { get; private set; }
        internal TerminalsConfigurationSection New { get; private set; }
    }
}
