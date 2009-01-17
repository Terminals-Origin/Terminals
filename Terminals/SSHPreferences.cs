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

namespace Terminals
{
	public enum AuthMethod {Host,Password,PublicKey,KeyboardInteractive};
	/// <summary>
	/// Description of SSHPreferences.
	/// </summary>
	public partial class SSHPreferences : UserControl
	{
		
		public SSHPreferences()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			buttonPublicKey.Checked = true;
			buttonSSH2.Checked = true;
			// TODO: move this to NewTerminalForm
			foreach(SSHKeyElement k in Settings.SSHKeys)
				comboBoxKey.Items.Add(k.tag);
			if(comboBoxKey.Items.Count>0)
				comboBoxKey.SelectedIndex = 0;
		}
		
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
       	public ComboBox.ObjectCollection Keys
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
					comboBoxKey.Items.Add(tag);
					comboBoxKey.SelectedIndex = comboBoxKey.FindString(tag);
				}
				try
				{
					Settings.AddSSHKey(tag, dlg.Key);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Duplicate Tag, please try again");
				}
			}
		}

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // copy the public key to the clipboard
            openSSHTextBox.Copy();
        }

        private void comboBoxKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tag = (string)comboBoxKey.SelectedItem;
            SSH2UserAuthKey key = Settings.SSHKeys[tag].key;
            openSSHTextBox.Text = key.PublicPartInOpenSSHStyle();
            //TypeDescriptor.GetConverter(key.GetType()).ConvertToString(key);
        }
	}
}
