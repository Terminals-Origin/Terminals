using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Terminals.Wizard
{
    internal static class SpecialCommandsWizard
    {
        /// <summary>
        /// Gets format string for jpg images under Thumbs directory.
        /// </summary>
        private const string THUMBS_FILENAME = @"Thumbs\{0}.jpg";

        private const string THUMBS_DIRECTORY = "Thumbs";

        private static SortedDictionary<string, string> KnownSpecialCommands = new SortedDictionary<string, string>();

        private static DirectoryInfo SystemRoot
        {
            get
            {
                return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System));
            }
        }

        internal static SpecialCommandConfigurationElementCollection LoadSpecialCommands()
        {
            SpecialCommandConfigurationElementCollection cmdList = new SpecialCommandConfigurationElementCollection();

            AddCmdCommand(cmdList);
            AddRegEditCommand(cmdList);
            AddMmcCommands(cmdList);
            AddControlPanelApplets(cmdList);

            return cmdList;
        }

        private static void AddControlPanelApplets(SpecialCommandConfigurationElementCollection cmdList)
        {
            string cpThumb = @"Thumbs\ControlPanel.png";
            if (!File.Exists(cpThumb))
            {
                Properties.Resources.ControlPanel.Save(cpThumb);
            }
            foreach (FileInfo file in SystemRoot.GetFiles("*.cpl"))
            {
                SpecialCommandConfigurationElement elm1 = new SpecialCommandConfigurationElement(file.Name);

                elm1.Thumbnail = cpThumb;

                Icon[] fileIcons = IconHandler.IconHandler.IconsFromFile(file.FullName, IconHandler.IconSize.Small);
                if (fileIcons != null && fileIcons.Length > 0)
                {
                    string t = string.Format(THUMBS_FILENAME, file.Name);
                    if (!File.Exists(t)) fileIcons[0].ToBitmap().Save(t);
                    elm1.Thumbnail = t;
                }

                string thumbName = string.Format(THUMBS_FILENAME, file.Name);
                if (File.Exists(thumbName))
                    elm1.Thumbnail = thumbName;

                elm1.Name = file.Name.Replace(file.Extension, "");
                elm1.Executable = @"%systemroot%\system32\" + file.Name;
                cmdList.Add(elm1);
            }
        }

        private static void AddMmcCommands(SpecialCommandConfigurationElementCollection cmdList)
        {
            Icon[] IconsList = IconHandler.IconHandler.IconsFromFile(Path.Combine(SystemRoot.FullName, "mmc.exe"),
                IconHandler.IconSize.Small);
            Random rnd = new Random();

            EnsureImagesDirectory();

            foreach (FileInfo file in SystemRoot.GetFiles("*.msc"))
            {
                MMC.MMCFile fileMMC = new MMC.MMCFile(file);
                
                SpecialCommandConfigurationElement elm1 = new SpecialCommandConfigurationElement(file.Name);

                if (IconsList != null && IconsList.Length > 0)
                {

                    string thumbName = string.Format(THUMBS_FILENAME, file.Name);
                    elm1.Thumbnail = thumbName;

                    if (!File.Exists(thumbName))
                    {
                        if (fileMMC.SmallIcon != null)
                        {
                            fileMMC.SmallIcon.ToBitmap().Save(thumbName);
                        }
                        else
                        {
                            IconsList[rnd.Next(IconsList.Length - 1)].ToBitmap().Save(thumbName);
                        }
                    }
                    if (fileMMC.Parsed)
                    {
                        elm1.Name = fileMMC.Name;
                    }
                    else
                    {
                        elm1.Name = file.Name.Replace(file.Extension, "");
                    }
                }

                elm1.Executable = @"%systemroot%\system32\" + file.Name;
                cmdList.Add(elm1);
            }
        }

        private static void AddRegEditCommand(SpecialCommandConfigurationElementCollection cmdList)
        {
            string regEditFile = Path.Combine(SystemRoot.FullName, "regedt32.exe");
            Icon[] regeditIcons = IconHandler.IconHandler.IconsFromFile(regEditFile, IconHandler.IconSize.Small);
            SpecialCommandConfigurationElement regEditElm = new SpecialCommandConfigurationElement("Registry Editor");
            if (regeditIcons != null && regeditIcons.Length > 0)
            {
                EnsureImagesDirectory();

                string thumbName = string.Format(@"Thumbs\regedt32.exe.jpg", regEditFile);
                if (!File.Exists(thumbName))
                    regeditIcons[0].ToBitmap().Save(thumbName);
                regEditElm.Thumbnail = thumbName;
            }

            regEditElm.Executable = regEditFile;
            cmdList.Add(regEditElm);
        }

        private static void EnsureImagesDirectory()
        {
            if (!Directory.Exists(THUMBS_DIRECTORY))
                Directory.CreateDirectory(THUMBS_DIRECTORY);
        }

        private static void AddCmdCommand(SpecialCommandConfigurationElementCollection cmdList)
        {
            SpecialCommandConfigurationElement elm = new SpecialCommandConfigurationElement("Command Shell");
            elm.Executable = @"%systemroot%\system32\cmd.exe";
            cmdList.Add(elm);
        }
    }
}