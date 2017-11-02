using System;
using System.IO;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Security;
using Terminals.Services;

namespace Terminals.Updates
{
    /// <summary>
    /// Class containing methods to update config files after an update. 
    /// </summary>
    internal class UpdateConfig
    {
        private readonly IPersistence persistence;

        private readonly Settings settings;

        private readonly ConnectionManager connectionManager;

        public UpdateConfig(Settings settings, IPersistence persistence, ConnectionManager connectionManager)
        {
            this.persistence = persistence;
            this.connectionManager = connectionManager;
            this.settings = settings;
        }

        /// <summary>
        /// Updates config file to current version, if it isn't up to date
        /// </summary>
        internal void CheckConfigVersionUpdate()
        {
            // If the Terminals version is not in the config or the version number in the config
            // is lower then the current assembly version, check for config updates
            if (settings.ConfigVersion == null || settings.ConfigVersion < Program.Info.Version)
            {
                // keep update sequence ordered!
                var v2 = new V20Upgrade(this.settings, this.persistence, this.connectionManager);
                v2.Upgrade();

                // After all updates change the config version to the current assembly version
                settings.ConfigVersion = Program.Info.Version;
            }
        }
    }
}
