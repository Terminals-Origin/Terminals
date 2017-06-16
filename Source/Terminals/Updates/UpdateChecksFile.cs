using System;
using System.Globalization;
using System.IO;
using Terminals.Configuration;

namespace Terminals.Updates
{
    /// <summary>
    /// Manipulations with file used to store last check for new releases.
    /// Doesn't contain protection against file IO exceptions.
    /// </summary>
    internal class UpdateChecksFile
    {
        private readonly string releaseFile = FileLocations.LastUpdateCheck;

        internal bool ShouldCheckForUpdate
        {
            get
            {
                if (File.Exists(releaseFile))
                {
                    DateTime lastUpdate = this.ReadLastUpdate();
                    if (lastUpdate.Date >= DateTime.UtcNow.Date)
                        return false;
                }
                return true;
            }
        }

        internal DateTime ReadLastUpdate()
        {
            String text = File.ReadAllText(this.releaseFile).Trim();
            DateTime lastUpdate = DateTime.MinValue;
            DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal,  out lastUpdate);
            return lastUpdate;
        }

        internal void WriteLastCheck()
        {
            string contents = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            File.WriteAllText(this.releaseFile, contents);
        }
    }
}
