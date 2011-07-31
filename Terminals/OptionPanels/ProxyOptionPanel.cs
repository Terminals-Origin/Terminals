using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Terminals.Forms
{
    internal class ProxyOptionPanel : OptionDialogCategoryPanel
    {
        private Panel panel1;
        private GroupBox groupBox6;
        private Label lblport;
        private TextBox ProxyPortTextbox;
        private TextBox ProxyAddressTextbox;
        private Label lblAddress;
        private RadioButton AutoProxyRadioButton;
        private RadioButton ProxyRadionButton;

        public ProxyOptionPanel()
        {
            InitializeComponent();
        }

        #region InitializeComponent
        
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblport = new System.Windows.Forms.Label();
            this.ProxyPortTextbox = new System.Windows.Forms.TextBox();
            this.ProxyAddressTextbox = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.AutoProxyRadioButton = new System.Windows.Forms.RadioButton();
            this.ProxyRadionButton = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 332);
            this.panel1.TabIndex = 27;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lblport);
            this.groupBox6.Controls.Add(this.ProxyPortTextbox);
            this.groupBox6.Controls.Add(this.ProxyAddressTextbox);
            this.groupBox6.Controls.Add(this.lblAddress);
            this.groupBox6.Controls.Add(this.AutoProxyRadioButton);
            this.groupBox6.Controls.Add(this.ProxyRadionButton);
            this.groupBox6.Location = new System.Drawing.Point(6, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(500, 98);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            // 
            // lblPort
            // 
            this.lblport.AutoSize = true;
            this.lblport.Location = new System.Drawing.Point(258, 70);
            this.lblport.Name = "label15";
            this.lblport.Size = new System.Drawing.Size(29, 13);
            this.lblport.TabIndex = 9;
            this.lblport.Text = "Port:";
            // 
            // ProxyPortTextbox
            // 
            this.ProxyPortTextbox.Location = new System.Drawing.Point(295, 67);
            this.ProxyPortTextbox.Name = "ProxyPortTextbox";
            this.ProxyPortTextbox.Size = new System.Drawing.Size(34, 20);
            this.ProxyPortTextbox.TabIndex = 8;
            this.ProxyPortTextbox.Text = "80";
            // 
            // ProxyAddressTextbox
            // 
            this.ProxyAddressTextbox.Location = new System.Drawing.Point(60, 66);
            this.ProxyAddressTextbox.Name = "ProxyAddressTextbox";
            this.ProxyAddressTextbox.Size = new System.Drawing.Size(192, 20);
            this.ProxyAddressTextbox.TabIndex = 7;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(3, 70);
            this.lblAddress.Name = "label14";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "Address:";
            // 
            // AutoProxyRadioButton
            // 
            this.AutoProxyRadioButton.AutoSize = true;
            this.AutoProxyRadioButton.Location = new System.Drawing.Point(6, 20);
            this.AutoProxyRadioButton.Name = "AutoProxyRadioButton";
            this.AutoProxyRadioButton.Size = new System.Drawing.Size(151, 17);
            this.AutoProxyRadioButton.TabIndex = 0;
            this.AutoProxyRadioButton.TabStop = true;
            this.AutoProxyRadioButton.Text = "Automatically Detect Proxy";
            this.AutoProxyRadioButton.UseVisualStyleBackColor = true;
            // 
            // ProxyRadionButton
            // 
            this.ProxyRadionButton.AutoSize = true;
            this.ProxyRadionButton.Location = new System.Drawing.Point(6, 43);
            this.ProxyRadionButton.Name = "ProxyRadionButton";
            this.ProxyRadionButton.Size = new System.Drawing.Size(172, 17);
            this.ProxyRadionButton.TabIndex = 1;
            this.ProxyRadionButton.TabStop = true;
            this.ProxyRadionButton.Text = "Use the following Proxy Server:";
            this.ProxyRadionButton.UseVisualStyleBackColor = true;
            // 
            // ProxyOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "ProxyOptionPanel";
            this.Size = new System.Drawing.Size(514, 332);
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public override void Init()
        {
            this.AutoProxyRadioButton.Checked = !Settings.UseProxy;
            this.ProxyRadionButton.Checked = Settings.UseProxy;
            this.ProxyAddressTextbox.Text = Settings.ProxyAddress;
            this.ProxyPortTextbox.Text = (Settings.ProxyPort.ToString().Equals("0")) ? "80" : Settings.ProxyPort.ToString();
            this.ProxyAddressTextbox.Enabled = Settings.UseProxy;
            this.ProxyPortTextbox.Enabled = Settings.UseProxy;
        }

        public override bool Save()
        {
            try
            {
                Settings.DelayConfigurationSave = true;

                Settings.UseProxy = this.ProxyRadionButton.Checked;
                Settings.ProxyAddress = this.ProxyAddressTextbox.Text;
                Settings.ProxyPort = Convert.ToInt32(this.ProxyPortTextbox.Text);

                return true;
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error(ex);
                return false;
            }
        }
    }
}
