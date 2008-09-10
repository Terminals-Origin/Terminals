using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

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
        public System.Drawing.Image LoadThumbnail()
        {
            System.Drawing.Image img = Terminals.Properties.Resources.application_xp_terminal;
            try
            {
                if (this.Thumbnail != null && this.Thumbnail != "")
                {
                    if (System.IO.File.Exists(this.Thumbnail)) img = System.Drawing.Image.FromFile(this.Thumbnail);
                }
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Could not LoadThumbnail for file:" + this.Thumbnail, exc); 
            }
            return img;
        }
        public void Launch()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            string exe = this.Executable;
            if (exe.Contains("%"))
            {
                exe = exe.Replace("%systemroot%", System.Environment.GetEnvironmentVariable("systemroot"));
            }
            p.StartInfo = new System.Diagnostics.ProcessStartInfo(exe, this.Arguments);
            p.StartInfo.WorkingDirectory = this.WorkingFolder;
            p.Start();
        }
    }
}