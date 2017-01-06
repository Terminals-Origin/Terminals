using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Terminals.Configuration;

namespace Terminals.Wizard
{
    internal static class SpecialCommandsWizard
    {
        /// <summary>
        /// Because it isn't easy to obtain the name of an applet.
        /// This holds atleast known applet names in a form of key=applet_executable, value=applet_name 
        /// </summary>
        private static readonly SortedDictionary<string, string> knownSystemControlApplets = new SortedDictionary<string, string>
            {
             {"access.cpl", "Accesibility Options"},
             {"appwiz.cpl", "Add or Remove programs"},
             {"bthprops.cpl", "Bluetooth Devices"},
             {"desk.cpl", "Display properties"},
             {"firewall.cpl", "Windows Firewall"},
             {"hdwwiz.cpl", "Add hardware wizard"},
             {"inetcpl.cpl", "Internet properties"},
             {"infocardcpl.cpl", "Info card"},
             {"intl.cpl", "Regional and language options"},
             {"irprops.cpl", "IR options"},
             {"joy.cpl", "Game controllers"},
             {"main.cpl", "Mouse properties"},
             {"mmsys.cpl", "Sounds and audio devices"},
             {"ncpa.cpl", "Network connections"},
             {"netsetup.cpl", "Wireless network setup wizard"},
             {"nusrmgr.cpl", "User accounts"},
             {"powercfg.cpl", "Power options properties"},
             {"sysdm.cpl", "System properties"},
             {"telephon.cpl", "Phone and Modem Options"},
             {"timedate.cpl", "Date and time properties"},
             {"wscui.cpl", "Windows security center"},
             {"wuaucpl.cpl", "Automatic updates"},
             {"mlcfg32.cpl", "Mail"},
             {"mlcfg.cpl","Mail"},
             {"Sapi.cpl", "Speech"},
             
             // thirt party
             {"igfxcpl.cpl", "Intel graphics media"},
             {"javacpl.cpl", "Java"},
             {"bdeadmin.cpl", "Borland database configuration"},
             {"nwc.cpl", "Client Service for NetWare (CSNW)"},
             {"odbccp32.cpl", "ODBC datasource administrator"},
             {"RTSndMgr.CPL", "Realtec sound options"},
             {"color.cpl", "Colors"},
             {"FlashPlayerCPLApp.cpl", "Flash Player"}
            };

        private static DirectoryInfo SystemRoot
        {
            get
            {
                return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System));
            }
        }

        internal static SpecialCommandConfigurationElementCollection LoadSpecialCommands()
        {
            var commands = new SpecialCommandConfigurationElementCollection();
            commands.Add(CreateCmdCommand());
            commands.Add(CreateRegEditCommand());
            AddMmcCommands(commands);
            AddControlPanelApplets(commands);
            return commands;
        }

        private static void AddControlPanelApplets(SpecialCommandConfigurationElementCollection commands)
        {
            string controlPanelImage = FileLocations.ControlPanelImage;
            if (!File.Exists(controlPanelImage))
            {
                SaveBitmap(Properties.Resources.ControlPanel, controlPanelImage);
            }

            foreach (FileInfo file in SystemRoot.GetFiles("*.cpl"))
            {
                var command = new SpecialCommandConfigurationElement(file.Name);
                command.Thumbnail = controlPanelImage;
                string thumbName = FileLocations.FormatThumbFileName(file.Name);

                Icon[] fileIcons = IconHandler.IconHandler.IconsFromFile(file.FullName, IconHandler.IconSize.Small);
                if (fileIcons != null && fileIcons.Length > 0)
                {
                    SaveIcon(fileIcons[0], thumbName);
                }
                
                if (File.Exists(thumbName))
                    command.Thumbnail = thumbName;

                command.Name = EnsureAppletName(file);
                command.Executable = @"%systemroot%\system32\" + file.Name;
                commands.Add(command);
            }
        }

        private static string EnsureAppletName(FileInfo appletFile)
        {
            if (knownSystemControlApplets.ContainsKey(appletFile.Name))
                return knownSystemControlApplets[appletFile.Name];

            return appletFile.Name.Replace(appletFile.Extension, "");
        }

        private static void AddMmcCommands(SpecialCommandConfigurationElementCollection commands)
        {
            Icon[] iconsList = IconHandler.IconHandler.IconsFromFile(Path.Combine(SystemRoot.FullName, "mmc.exe"),
                IconHandler.IconSize.Small);
            Random rnd = new Random();
            FileLocations.EnsureImagesDirectory();

            foreach (FileInfo file in SystemRoot.GetFiles("*.msc"))
            {
                MMC.MMCFile fileMMC = new MMC.MMCFile(file);
                var command = new SpecialCommandConfigurationElement(file.Name);

                if (iconsList != null && iconsList.Length > 0)
                {
                    string thumbName = FileLocations.FormatThumbFileName(file.Name);
                    command.Thumbnail = thumbName;

                    if (fileMMC.SmallIcon != null)
                    {
                        SaveIcon(fileMMC.SmallIcon, thumbName);
                    }
                    else
                    {
                        SaveIcon(iconsList[rnd.Next(iconsList.Length - 1)], thumbName);
                    }

                    if (fileMMC.Parsed)
                    {
                        command.Name = fileMMC.Name;
                    }
                    else
                    {
                        command.Name = file.Name.Replace(file.Extension, "");
                    }
                }

                command.Executable = @"%systemroot%\system32\" + file.Name;
                commands.Add(command);
            }
        }

        private static SpecialCommandConfigurationElement CreateRegEditCommand()
        {
            const string REG_EDIT_EXE = "regedt32.exe";
            string regEditFile = Path.Combine(SystemRoot.FullName, REG_EDIT_EXE);
            Icon[] regeditIcons = IconHandler.IconHandler.IconsFromFile(regEditFile, IconHandler.IconSize.Small);
            var command = new SpecialCommandConfigurationElement("Registry Editor");
            
            if (regeditIcons != null && regeditIcons.Length > 0)
            {
                FileLocations.EnsureImagesDirectory();
                string thumbName = FileLocations.FormatThumbFileName(REG_EDIT_EXE);
                SaveIcon(regeditIcons[0], thumbName);
                command.Thumbnail = thumbName;
            }

            command.Executable = regEditFile;
            return command;
        }

        private static void SaveIcon(Icon iconToSave, string fullThumbFileName)
        {
            SaveBitmap(iconToSave.ToBitmap(), fullThumbFileName);
        }

        private static void SaveBitmap(Image imageToSave, string fullThumbFileName)
        {
            try
            {
                if (!File.Exists(fullThumbFileName))
                    imageToSave.Save(fullThumbFileName);
            }
            catch (Exception)
            {
                // nothing to recover, the icon is optional
                Logging.Info("Unable to create icon for command: " + fullThumbFileName);
            }
        }

        private static SpecialCommandConfigurationElement CreateCmdCommand()
        {
            var command = new SpecialCommandConfigurationElement("Command Shell");
            command.Executable = @"%systemroot%\system32\cmd.exe";
            return command;
        }
    }
}