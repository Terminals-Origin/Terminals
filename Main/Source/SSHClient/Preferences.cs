using System;
using System.Windows.Forms;

namespace SSHClient
{
	/// <summary>
	/// Description of Preferences.
	/// </summary>
	public partial class Preferences : UserControl
	{
		public Preferences()
		{
			InitializeComponent();
			
			buttonPublicKey.Checked = true;
			buttonSSH2.Checked = true;
            
		}
		
		public KeysSection Keys
		{
			set
			{
				keysSection = value;
				foreach(KeyConfigElement k in keysSection.Keys)
					comboBoxKey.Items.Add(k.Name);
				if(comboBoxKey.Items.Count>0)
					comboBoxKey.SelectedIndex = 0;
			}
		}

		public KeysSection keysSection;

        public AuthMethod AuthMethod
        {
            get
            {
                if (buttonPublicKey.Checked)
                    return AuthMethod.PublicKey;
                if (buttonPassword.Checked)
                    return AuthMethod.Password;

                return AuthMethod.KeyboardInteractive;
            }
            set
            {
                switch (value)
                {
                    case AuthMethod.Password:
                        buttonPassword.Checked = true;
                        break;
                    case AuthMethod.PublicKey:
                        buttonPublicKey.Checked = true;
                        break;
                    default:
                        buttonKbd.Checked = true;
                        break;
                }
            }
        }
		public bool SSH1
		{
			get{ return buttonSSH1.Checked;}
			set{ buttonSSH1.Checked=value;}
		}
        public string KeyTag
        {
        	get
        	{
        		return comboBoxKey.Text;
        	}
        	set
        	{
        		comboBoxKey.Text = value;
        	}
        }
       	public ComboBox.ObjectCollection KeyTags
        {
        	get
        	{
        		return comboBoxKey.Items;
        	}
        }
		
		void ButtonGenerateKeyClick(object sender, EventArgs e)
		{
			if(SSH1)
			{
				LoadSSH1Key();
			}
			else
			{
				GenerateSSH2Key();
			}
		}
		
		void LoadSSH1Key()
		{
			MessageBox.Show("not done yet");
		}
		
		void GenerateSSH2Key()
		{
			KeyGenForm dlg = new KeyGenForm();
			dlg.KeyTag = comboBoxKey.Text;
			dlg.ShowDialog();
			if(dlg.Key != null)
			{
				string tag = dlg.KeyTag;
				int i = comboBoxKey.FindString(tag);
				if(i>=0)
				{
					comboBoxKey.SelectedIndex = i;
				}
				else
				{
					keysSection.AddKey(tag, dlg.Key.toBase64String());
					comboBoxKey.Items.Add(tag);
					comboBoxKey.SelectedIndex = comboBoxKey.FindString(tag);
				}
			}
		}

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // copy the public key to the clipboard
            openSSHTextBox.SelectAll();
            openSSHTextBox.Copy();
        }

        private void comboBoxKey_SelectedIndexChanged(object sender, EventArgs e)
        {
        	// TODO SSH1 ?
            string tag = (string)comboBoxKey.SelectedItem;
            if (!string.IsNullOrEmpty(tag))
            {
                string keytext = keysSection.Keys[tag].Key;
                SSH2UserAuthKey key = SSH2UserAuthKey.FromBase64String(keytext);
                openSSHTextBox.Text = key.PublicPartInOpenSSHStyle() + " " + tag;
            }
        }

		void ButtonSSH1CheckedChanged(object sender, EventArgs e)
		{
			if(buttonSSH1.Checked)
			{
				buttonGenerateKey.Text = "Load";
			}
			else
			{
				// TODO import putty keys
				buttonGenerateKey.Text = "New";				
			}
		}

        private void button1_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog())
            {
                string home = System.Environment.GetEnvironmentVariable("HOME");
                if(string.IsNullOrEmpty(home)) home = string.Format("{0}{1}", System.Environment.GetEnvironmentVariable("HOMEDRIVE"), System.Environment.GetEnvironmentVariable("HOMEPATH"));
                var sshFolder = System.IO.Path.Combine(home, ".ssh");
                if (System.IO.Directory.Exists(sshFolder)) home = sshFolder;

                dlg.InitialDirectory = home;
                dlg.CheckFileExists = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.ssh2PrivateKeyPath.Text = dlg.FileName;
                    buttonPublicKey.Checked = true;
                    openSSHTextBox.Text = "";
                    comboBoxKey.SelectedIndex = -1;
                    comboBoxKey.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBoxKey.SelectedIndex = -1;
            openSSHTextBox.Text = "";
        }

        public string SSHKeyFile { get { return this.ssh2PrivateKeyPath.Text; } set
        {
            this.ssh2PrivateKeyPath.Text = value;
            if (!string.IsNullOrEmpty(value))
            {
                this.comboBoxKey.SelectedIndex = -1;
                this.KeyTag = "";
                this.openSSHTextBox.Text = "";
            }
        } }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var msg = "This key needs to be a 'ssh.com' key.  Use PuTTY Key Generator to convert your existing keyfiles to this format.";
            MessageBox.Show(msg);
        }
	}
}
