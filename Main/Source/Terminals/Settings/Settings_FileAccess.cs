using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Terminals.Data;
using Terminals.Properties;
using SysConfig = System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Terminals.Configuration
{
    internal delegate void ConfigurationChangedHandler(ConfigurationChangedEventArgs args);

    internal static partial class Settings
    {
        /// <summary>
        /// Informs lisseners, that configuration file was changed by another application
        /// or another Terminals instance. In this case all cached not saved data are lost.
        /// </summary>
        internal static event ConfigurationChangedHandler ConfigurationChanged;

        private static FileLocations fileLocations = new FileLocations();
        internal static FileLocations FileLocations
        {
            get { return fileLocations; }
        }

        private static System.Configuration.Configuration _config = null;
        private static System.Configuration.Configuration Config
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

        private static void ConfigFileChanged(object sender, EventArgs e)
        {
            TerminalsConfigurationSection old = GetSection();
            ForceReload();
            var args = ConfigurationChangedEventArgs.CreateFromSettings(old, GetSection());
            FireConfigurationChanged(args);
        }

        private static void FireConfigurationChanged(ConfigurationChangedEventArgs args)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(args);
            }
        }

        private static void InitializeFileWatcher()
        {
            if (fileWatcher != null)
                return;

            fileWatcher = new DataFileWatcher(fileLocations.Configuration);
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
            Debug.WriteLine(String.Format("Terminals.config file saved."));
        }

        private static System.Configuration.Configuration GetConfiguration()
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
            if (!File.Exists(fileLocations.Configuration))
                SaveDefaultConfigFile();
        }

        private static ExeConfigurationFileMap CreateConfigFileMap()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = fileLocations.Configuration;
            return configFileMap;
        }

        private static System.Configuration.Configuration OpenConfiguration()
        {
            ExeConfigurationFileMap configFileMap = CreateConfigFileMap();
            fileLock.WaitOne();
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            fileLock.ReleaseMutex();
            return config;
        }

        private static void BackUpConfigFile()
        {
            if (File.Exists(fileLocations.Configuration))
            {
                string backupFileName = GetBackupFileName();
                // back it up before we do anything
                File.Copy(fileLocations.Configuration, backupFileName);
                // now delete it
                File.Delete(fileLocations.Configuration);
            }
        }

        private static string GetBackupFileName()
        {
            string newGUID = Guid.NewGuid().ToString();
            long fileDate = DateTime.Now.ToFileTime();
            string backupFile = String.Format("Terminals-{1}-{0}.config", newGUID, fileDate);
            return FileLocations.GetFullPath(backupFile);
        }

        private static void SaveDefaultConfigFile()
        {
            string templateConfigFile = Resources.Terminals;
            using (StreamWriter sr = new StreamWriter(fileLocations.Configuration))
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

        private static System.Configuration.Configuration ImportConfiguration()
        {
            // get a temp filename to hold the current settings which are failing
            string tempFile = Path.GetTempFileName();

            fileWatcher.StopObservation();
            MoveAndDeleteFile(fileLocations.Configuration, tempFile);
            SaveDefaultConfigFile();
            fileWatcher.StartObservation();
            System.Configuration.Configuration c = OpenConfiguration();

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
                    System.Configuration.Configuration configuration = ImportConfiguration();
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