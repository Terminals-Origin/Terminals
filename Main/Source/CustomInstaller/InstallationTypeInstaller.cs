using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
                CheckPortableInstallType();
            }
            catch (Exception exception)
            {
                throw new InstallException("Installer was unable to update config file.", exception);
            }
        }

        private void CheckPortableInstallType()
        {
            if (InstallToUserProfile())
            {
                string targetDir = this.Context.Parameters["TargetDir"];
                UpdateConfigFile(targetDir);
                UpdateLog4NetLogDirectory(targetDir);
            }
        }

        private bool InstallToUserProfile()
        {
            string portableParam = this.Context.Parameters["InstallType"];
            int numericValue = Convert.ToInt32(portableParam);
            return Convert.ToBoolean(numericValue);
        }

        private static void UpdateConfigFile(string targetDir)
        {
            string configFilePath = Path.Combine(targetDir, "Terminals.exe.config");
            XDocument configFile = XDocument.Load(configFilePath);
            XElement portalbeElement = SelectPortableElement(configFile);
            if (portalbeElement != null)
                portalbeElement.Value = false.ToString();

            configFile.Save(configFilePath);
        }

        private static XElement SelectPortableElement(XDocument config)
        {
            XElement portableElement = config.Descendants("Terminals.Properties.Settings")
                .Elements("setting")
                .Where(IsPortableSetting)
                .FirstOrDefault();

            if (portableElement != null)
                return portableElement.Element("value");

            return null;
        }

        private static bool IsPortableSetting(XElement setting)
        {
            XAttribute name = setting.Attributes("name").FirstOrDefault();
            if (name != null)
                return name.Value == "Portable";
            return false;
        }

        private static void UpdateLog4NetLogDirectory(string targetDir)
        {
            string log4NetFilePath = Path.Combine(targetDir, "Terminals.log4net.config");
            XDocument configFile = XDocument.Load(log4NetFilePath);
            XAttribute fileAttribute = SelectFileElement(configFile);

            string logDirectoryPath = GetLogDirectoryPath();
            if (fileAttribute != null)
                fileAttribute.Value = logDirectoryPath;

            configFile.Save(log4NetFilePath);
        }

        private static string GetLogDirectoryPath()
        {
            return @"${USERPROFILE}\Local Settings\Application Data\Robert_Chartier\Terminals\Data\logs\CurrentLog.txt";
        }

        private static XAttribute SelectFileElement(XDocument configFile)
        {
            return configFile.Descendants("file")
                .Attributes("value")
                .FirstOrDefault();
        }
    }
}
