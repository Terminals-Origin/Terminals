using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace Terminals
{
    [RunInstaller(true)]
    public partial class InstallationTypeInstaller : Installer
    {
        public InstallationTypeInstaller()
        {
            InitializeComponent();
        }

        public override void Commit(IDictionary savedState)
        {
            try
            {
                base.Commit(savedState);
                bool installToUserProfile = InstallToUserProfile();
                string targetDir = this.Context.Parameters["TargetDir"];
                UpgradeConfigFiles.CheckPortableInstallType(targetDir, installToUserProfile);
            }
            catch (Exception exception)
            {
                throw new InstallException("Installer was unable to update config files.", exception);
            }
        }

        private bool InstallToUserProfile()
        {
            string portableParam = this.Context.Parameters["InstallType"];
            int numericValue = Convert.ToInt32(portableParam);
            return Convert.ToBoolean(numericValue);
        }
    }
}
