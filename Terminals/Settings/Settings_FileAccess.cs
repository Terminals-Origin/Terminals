using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Terminals.Data;
using SysConfig = System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Terminals.Configuration
{
    internal delegate void ConfigFileReloadedHandler(ConfigFileChangedEventArgs args);

    internal static partial class Settings
    {
        internal const String CONFIG_FILE_NAME = "Terminals.config";

        private static string configurationFileLocation;
        internal static string ConfigurationFileLocation
        {
            get
            {
                if (string.IsNullOrEmpty(configurationFileLocation))
                    SetDefaultConfigurationFileLocation();

                return configurationFileLocation;
            }
            set
            {
                configurationFileLocation = value;
                if (fileWatcher != null)
                    fileWatcher.FullFileName = value;
            }
        }

        private static SysConfig.Configuration _config = null;
        private static DataFileWatcher fileWatcher;

        /// <summary>
        /// Prevent concurent updates on config file by another program
        /// </summary>
        private static Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.Settings");

        /// <summary>
        /// Flag informing, that configuration shouldnt be saved imediately, but after explicit call
        /// This increases performance for 
        /// </summary>
        private static bool delayConfigurationSave;

        /// <summary>
        /// Informs lisseners, that configuration file was changed by another application
        /// or another Terminals instance. In this case all cached not saved data are lost.
        /// </summary>
        internal static event ConfigFileReloadedHandler ConfigFileReloaded;

        private static void SetDefaultConfigurationFileLocation()
        {
            string assemblyDirectory = GetAssemblyDirectory();
            configurationFileLocation = Path.Combine(assemblyDirectory, CONFIG_FILE_NAME);
        }

        /// <summary>
        /// Gets the executing assembly directory full path without last basckslash.
        /// </summary>
        internal static string GetAssemblyDirectory()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(assemblyLocation);
        }

        private static void ConfigFileChanged(object sender, EventArgs e)
        {
            TerminalsConfigurationSection old = GetSection();
            ForceReload();
            if (ConfigFileReloaded != null)
            {
                var args = new ConfigFileChangedEventArgs(old, GetSection());
                ConfigFileReloaded(args);
            }
        }

        private static SysConfig.Configuration Config
        {
            get
            {
                if (_config == null)
                {
                    InitializeFileWatcher();
                    _config = GetConfiguration();
                }

                return _config;
            }
        }

        private static void InitializeFileWatcher()
        {
            if (fileWatcher != null)
                return;

            fileWatcher = new DataFileWatcher(ConfigurationFileLocation);
            fileWatcher.FileChanged += new EventHandler(ConfigFileChanged);
        }

        /// <summary>
        /// Because filewatcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal static void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
           fileWatcher.AssignSynchronizer(synchronizer); 
        }

        internal static void ForceReload()
        {
            _config = GetConfiguration();
        }

        /// <summary>
        /// Prevents save configuration after each change. After this call, no settings are saved
        /// into config file, until you call SaveAndFinishDelayedUpdate.
        /// This dramatically increases performance. Use this method for batch updates.
        /// </summary>
        internal static void StartDelayedUpdate()
        {
            delayConfigurationSave = true;
        }

        /// <summary>
        /// Stops prevent write changes into config file and immediately writes last state.
        /// Usually the changes are saved immediately
        /// </summary>
        internal static void SaveAndFinishDelayedUpdate()
        {
            delayConfigurationSave = false;
            SaveImmediatelyIfRequested();
        }

        private static void SaveImmediatelyIfRequested()
        {
            if (!delayConfigurationSave)
            {
                try
                {
                    fileLock.WaitOne();  // lock the file for changes by other application instance
                    Save();
                }
                catch (Exception exception)
                {
                    Logging.Log.Error("Config file access failed by save.", exception);
                }
                finally
                {
                    fileLock.ReleaseMutex();
                }
            }
        }

        private static void Save()
        {
            fileWatcher.StopObservation();
            Config.Save();
            fileWatcher.StartObservation();
            Debug.WriteLine(string.Format("Termianls.config file saved."));
        }

        private static SysConfig.Configuration GetConfiguration()
        {
            try
            {
                CreateConfigFileIfNotExist();
                return OpenConfiguration();
            }
            catch (Exception exc) // try to recover the file
            {
                Logging.Log.Error("Get Configuration", exc);
                BackUpConfigFile();
                SaveDefaultConfigFile();
                return OpenConfiguration();
            }
        }

        private static void CreateConfigFileIfNotExist()
        {
            if (!File.Exists(ConfigurationFileLocation))
                SaveDefaultConfigFile();
        }

        private static SysConfig.ExeConfigurationFileMap CreateConfigFileMap()
        {
            SysConfig.ExeConfigurationFileMap configFileMap = new SysConfig.ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = ConfigurationFileLocation;
            return configFileMap;
        }

        private static SysConfig.Configuration OpenConfiguration()
        {
            SysConfig.ExeConfigurationFileMap configFileMap = CreateConfigFileMap();
            fileLock.WaitOne();
            SysConfig.Configuration config = SysConfig.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, SysConfig.ConfigurationUserLevel.None);
            fileLock.ReleaseMutex();
            return config;
        }

        private static void BackUpConfigFile()
        {
            if (File.Exists(ConfigurationFileLocation))
            {
                string backupFileName = GetBackupFileName();
                // back it up before we do anything
                File.Copy(ConfigurationFileLocation, backupFileName);
                // now delete it
                File.Delete(ConfigurationFileLocation);
            }
        }

        private static string GetBackupFileName()
        {
            string newGUID = Guid.NewGuid().ToString();
            string folder = Path.GetDirectoryName(ConfigurationFileLocation);
            long fileDate = DateTime.Now.ToFileTime();
            string backupFile = string.Format("Terminals-{1}-{0}.config", newGUID, fileDate);
            return Path.Combine(folder, backupFile);
        }

        private static void SaveDefaultConfigFile()
        {
            string templateConfigFile = Properties.Resources.Terminals;
            using (StreamWriter sr = new StreamWriter(ConfigurationFileLocation))
            {
                sr.Write(templateConfigFile);
            }
        }

        private static void MoveAndDeleteFile(string fileName, string tempFileName)
        {
            // delete the zerobyte file which is created by default
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);

            // move the error file to the temp file
            File.Move(fileName, tempFileName);

            // if its still hanging around, kill it
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        private static SysConfig.Configuration ImportConfiguration()
        {
            // get a temp filename to hold the current settings which are failing
            string tempFile = Path.GetTempFileName();

            fileWatcher.StopObservation();
            MoveAndDeleteFile(ConfigurationFileLocation, tempFile);
            SaveDefaultConfigFile();
            fileWatcher.StartObservation();
            SysConfig.Configuration c = OpenConfiguration();

            // get a list of the properties on the Settings object (static props)
            PropertyInfo[] propList = typeof(Settings).GetProperties();

            // read all the xml from the erroring file
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(tempFile));

            // get the settings root
            XmlNode root = doc.SelectSingleNode("/configuration/settings");
            try
            {
                // for each setting's attribute
                foreach (XmlAttribute att in root.Attributes)
                {
                    // scan for the related property if any
                    try
                    {
                        foreach (PropertyInfo info in propList)
                        {
                            try
                            {
                                if (info.Name.ToLower() == att.Name.ToLower())
                                {
                                    // found a matching property, try to set it
                                    string val = att.Value;
                                    info.SetValue(null, Convert.ChangeType(val, info.PropertyType), null);
                                    break;
                                }
                            }
                            catch (Exception exc)
                            { // ignore the error
                                Logging.Log.Error("Remapping Settings Inner", exc);
                            }
                        }
                    }
                    catch (Exception exc) // ignore the error
                    {
                        Logging.Log.Error("Remapping Settings Outer", exc);
                    }
                }
            }
            catch (Exception exc) // ignore the error
            {
                Logging.Log.Error("Remapping Settings Outer Try", exc);
            }

            XmlNodeList favs = doc.SelectNodes("/configuration/settings/favorites/add");
            try
            {
                foreach (XmlNode fav in favs)
                {
                    try
                    {
                        FavoriteConfigurationElement newFav = new FavoriteConfigurationElement();
                        foreach (XmlAttribute att in fav.Attributes)
                        {
                            try
                            {
                                foreach (PropertyInfo info in newFav.GetType().GetProperties())
                                {
                                    try
                                    {
                                        if (info.Name.ToLower() == att.Name.ToLower())
                                        {
                                            // found a matching property, try to set it
                                            string val = att.Value;
                                            if (info.PropertyType.IsEnum)
                                            {
                                                info.SetValue(newFav, Enum.Parse(info.PropertyType, val), null);
                                            }
                                            else
                                            {
                                                info.SetValue(newFav, Convert.ChangeType(val, info.PropertyType), null);
                                            }

                                            break;
                                        }
                                    }
                                    catch (Exception exc) // ignore the error
                                    {
                                        Logging.Log.Error("Remapping Favorites 1", exc);
                                    }
                                }
                            }
                            catch (Exception exc) // ignore the error
                            {
                                Logging.Log.Error("Remapping Favorites 2", exc);
                            }
                        }

                        AddFavorite(newFav);
                    }
                    catch (Exception exc) // ignore the error
                    {
                        Logging.Log.Error("Remapping Favorites 3", exc);
                    }
                }
            }
            catch (Exception exc) // ignore the error
            {
                Logging.Log.Error("Remapping Favorites 4", exc);
            }

            return c;
        }

        private static TerminalsConfigurationSection GetSection()
        {
            try
            {
                return Config.GetSection("settings") as TerminalsConfigurationSection;
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("telnet"))
                {
                    MessageBox.Show("You need to replace telnetrows, telnetcols, telnetfont, telnetbackcolor, "
                    + "telnettextcolor, telnetcursorcolor with consolerows, consolecols, consolefont, consolebackcolor, "
                    + "consoletextcolor, consolecursorcolor");
                    return null;
                }

                Logging.Log.Info("Telnet Section Failed", exc);

                try
                {
                    // kick into the import routine
                    SysConfig.Configuration configuration = ImportConfiguration();
                    configuration = GetConfiguration();
                    if (configuration == null)
                        MessageBox.Show("Terminals was able to automatically upgrade your existing connections.");
                    return configuration.GetSection("settings") as TerminalsConfigurationSection;
                }
                catch (Exception importException)
                {
                    Logging.Log.Info("Trying to import connections failed", importException);
#if !DEBUG
                    string message = string.Format("Terminals was NOT able to automatically upgrade your existing connections.\r\nError:{0}",
                        importException.Message);
                    MessageBox.Show(message);
#endif
                    return new TerminalsConfigurationSection();
                }
            }
        }
    }
}