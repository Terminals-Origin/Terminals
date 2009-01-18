/*
 * Created by SharpDevelop.
 * User: cablej01
 * Date: 03/11/2008
 * Time: 18:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SSHClient
{
    public enum AuthMethod {Host,Password,PublicKey,KeyboardInteractive};

	/// <summary>
	/// Description of Preferences.
	/// </summary>
	public partial class Preferences : UserControl
	{
		
		public Preferences()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
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
				if(buttonPublicKey.Checked) return AuthMethod.PublicKey;
				if(buttonPassword.Checked) return AuthMethod.Password;
				if(buttonKbd.Checked) return AuthMethod.KeyboardInteractive;
				return AuthMethod.Host;
			}
			set
			{
				switch(value)
				{
					case AuthMethod.Password:
						buttonPassword.Checked = true;
						break;
					case AuthMethod.PublicKey:
						buttonPublicKey.Checked = true;
						break;
					case AuthMethod.KeyboardInteractive:
						buttonKbd.Checked = true;
						break;
					case AuthMethod.Host:
						buttonHost.Checked = true;
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
					string s = dlg.Key.toSECSHStyle(tag);
					keysSection.AddKey(tag, s);
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
            string tag = (string)comboBoxKey.SelectedItem;
            string keytext = keysSection.Keys[tag].Key;
            SSH2UserAuthKey key = SSH2UserAuthKey.FromSECSHStyle(keytext);
            openSSHTextBox.Text = key.PublicPartInOpenSSHStyle()+" "+tag;
        }
	}
}
