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
using System.Globalization;
//using System.Text;
using System.IO;

namespace SSHClient
{
    public class SSH2UserAuthKey : Routrek.SSHCV2.SSH2UserAuthKey
    {
        public SSH2UserAuthKey(Routrek.PKI.KeyPair kp) : base(kp)
        {
        }
        public string PublicPartInOpenSSHStyle()
        {
            Byte[] mk = new Byte[2048];
            MemoryStream ks = new MemoryStream(mk);
            base.WritePublicPartInOpenSSHStyle(ks);
            ks = new MemoryStream(mk);
            StreamReader sr = new StreamReader(ks);
            return sr.ReadLine();
        }
        
        public static SSH2UserAuthKey FromSECSHStyle(string value)
        {
            Byte[] mk = new Byte[2000];
            MemoryStream ks = new MemoryStream(mk);
            StreamWriter sw = new StreamWriter(ks);
            sw.WriteLine(value);
            ks = new MemoryStream(mk);
            Routrek.SSHCV2.SSH2UserAuthKey k = SSH2UserAuthKey.FromSECSHStyleStream(ks, "");
            return k as SSH2UserAuthKey;
        }
    }
    
    public enum AuthMethod {Host,Password,PublicKey,KeyboardInteractive};
	/// <summary>
	/// Description of SSHPreferences.
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
					keysSection.AddKey(tag, dlg.Key.ToString());
					comboBoxKey.Items.Add(tag);
					comboBoxKey.SelectedIndex = comboBoxKey.FindString(tag);
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
            SSH2UserAuthKey key = SSH2UserAuthKey.FromSECSHStyle(keysSection.Keys[tag].Key);
            openSSHTextBox.Text = key.PublicPartInOpenSSHStyle();
        }
	}
}
