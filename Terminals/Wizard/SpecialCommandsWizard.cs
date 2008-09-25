using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Wizard
{
    class SpecialCommandsWizard
    {

        private static SortedDictionary<string, string> KnownSpecialCommands = new SortedDictionary<string, string>();
        static SpecialCommandsWizard()
        {

        }
        public static SpecialCommandConfigurationElementCollection LoadSpecialCommands()
        {
            SpecialCommandConfigurationElementCollection cmdList = new SpecialCommandConfigurationElementCollection();
            //add the command prompt
            SpecialCommandConfigurationElement elm = new SpecialCommandConfigurationElement("Command Shell");
            elm.Executable = @"%systemroot%\system32\cmd.exe";
            cmdList.Add(elm);

            System.IO.DirectoryInfo systemroot = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System));

            string regEditFile = System.IO.Path.Combine(systemroot.FullName, "regedt32.exe");
            Icon[] regeditIcons = IconHandler.IconHandler.IconsFromFile(regEditFile, IconHandler.IconSize.Small);
            SpecialCommandConfigurationElement regEditElm = new SpecialCommandConfigurationElement("Registry Editor");
            if (regeditIcons != null && regeditIcons.Length > 0)
            {
                if (!System.IO.Directory.Exists("Thumbs")) System.IO.Directory.CreateDirectory("Thumbs");
                string thumbName = string.Format(@"Thumbs\regedt32.exe.jpg", regEditFile);
                if (!System.IO.File.Exists(thumbName)) regeditIcons[0].ToBitmap().Save(thumbName);
                regEditElm.Thumbnail = thumbName;
            }

            //elm1.Thumbnail = "";
            regEditElm.Executable = regEditFile;
            cmdList.Add(regEditElm);

            Icon[] IconsList = IconHandler.IconHandler.IconsFromFile(System.IO.Path.Combine(systemroot.FullName, "mmc.exe"), IconHandler.IconSize.Small);
            System.Random rnd = new Random();

            if (!System.IO.Directory.Exists("Thumbs")) System.IO.Directory.CreateDirectory("Thumbs");

            foreach (System.IO.FileInfo file in systemroot.GetFiles("*.msc"))
            {
                MMC.MMCFile fileMMC = new Terminals.Wizard.MMC.MMCFile(file);
                
                SpecialCommandConfigurationElement elm1 = new SpecialCommandConfigurationElement(file.Name);

                if (IconsList != null && IconsList.Length > 0)
                {

                    string thumbName = string.Format(@"Thumbs\{0}.jpg", file.Name);
                    elm1.Thumbnail = thumbName;

                    if (!System.IO.File.Exists(thumbName))
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

                //elm1.Thumbnail = "";
                elm1.Executable = @"%systemroot%\system32\" + file.Name;
                cmdList.Add(elm1);
            }
            string cpThumb = @"Thumbs\ControlPanel.jpg";
            if (!System.IO.File.Exists(cpThumb))
            {
                global::Terminals.Properties.Resources.ControlPanel.Save(cpThumb);
            }
            foreach (System.IO.FileInfo file in systemroot.GetFiles("*.cpl"))
            {
                SpecialCommandConfigurationElement elm1 = new SpecialCommandConfigurationElement(file.Name);

                elm1.Thumbnail = cpThumb;

                Icon[] fileIcons = IconHandler.IconHandler.IconsFromFile(file.FullName, IconHandler.IconSize.Small);
                if (fileIcons != null && fileIcons.Length > 0)
                {
                    string t = string.Format(@"Thumbs\{0}.jpg", file.Name);
                    if (!System.IO.File.Exists(t)) fileIcons[0].ToBitmap().Save(t);
                    elm1.Thumbnail = t;
                }

                string thumbName = string.Format(@"Thumbs\{0}.jpg", file.Name);
                if (System.IO.File.Exists(thumbName)) elm1.Thumbnail = thumbName;
                elm1.Name = file.Name.Replace(file.Extension, "");
                elm1.Executable = @"%systemroot%\system32\" + file.Name;
                cmdList.Add(elm1);
            }

            return cmdList;

        }
    }
}