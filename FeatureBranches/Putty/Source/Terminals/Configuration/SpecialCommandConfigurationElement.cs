using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Terminals.Properties;

namespace Terminals
{
    public class SpecialCommandConfigurationElement : ConfigurationElement
    {
        public SpecialCommandConfigurationElement()
        {
        }

        public SpecialCommandConfigurationElement(string name)
        {
            Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("executable", IsRequired = false)]
        public string Executable
        {
            get
            {
                return (string)this["executable"];
            }
            set
            {
                this["executable"] = value;
            }
        }

        [ConfigurationProperty("arguments", IsRequired = false)]
        public string Arguments
        {
            get
            {
                return (string)this["arguments"];
            }
            set
            {
                this["arguments"] = value;
            }
        }

        [ConfigurationProperty("workingFolder", IsRequired = false)]
        public string WorkingFolder
        {
            get
            {
                return (string)this["workingFolder"];
            }
            set
            {
                this["workingFolder"] = value;
            }
        }

        [ConfigurationProperty("thumbnail", IsRequired = false)]
        public string Thumbnail
        {
            get
            {
                return (string)this["thumbnail"];
            }
            set
            {
                this["thumbnail"] = value;
            }
        }

        public Image LoadThumbnail()
        {
            Image img = Resources.application_xp_terminal;
            try
            {
                if (!string.IsNullOrEmpty(this.Thumbnail))
                {
                    if (File.Exists(this.Thumbnail)) img = Image.FromFile(this.Thumbnail);
                }
            }
            catch (Exception exc)
            {
                Logging.Error("Could not LoadThumbnail for file:" + this.Thumbnail, exc); 
            }
            return img;
        }

        public void Launch()
        {
            try
            {
                this.TryLaunch();
            }
            catch (Exception ex)
            {
                string message = String.Format("Could not Launch the shortcut application: '{0}'", this.Name);
                MessageBox.Show(message);
                Logging.Error(message, ex);
            }
        }

        private void TryLaunch()
        {
            string exe = this.Executable;
            if (exe.Contains("%"))
                exe = exe.Replace("%systemroot%", Environment.GetEnvironmentVariable("systemroot"));

            var startInfo = new ProcessStartInfo(exe, this.Arguments);
            startInfo.WorkingDirectory = this.WorkingFolder;
            Process.Start(startInfo);
        }
    }
}