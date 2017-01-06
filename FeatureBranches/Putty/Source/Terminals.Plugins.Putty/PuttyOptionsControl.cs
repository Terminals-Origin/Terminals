using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Plugins.Putty
{
    public partial class PuttyOptionsControl : UserControl, IProtocolOptionsControl
    {
        private Label labelSession;
        private CheckBox checkBoxX11Forwarding;
        private CheckBox checkBoxCompression;
        private ComboBox cmbSessionName;

        public PuttyOptionsControl()
        {
            InitializeComponent();
        }

        public void LoadFrom(IFavorite favorite)
        {
            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (null != puttyOptions)
            {
                this.checkBoxX11Forwarding.Checked = puttyOptions.X11Forwarding;
                this.checkBoxCompression.Checked = puttyOptions.EnableCompression;
                this.cmbSessionName.Text = puttyOptions.SessionName;
            }
        }

        public void SaveTo(IFavorite favorite)
        {
            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (null != puttyOptions)
            {
                puttyOptions.X11Forwarding = this.checkBoxX11Forwarding.Checked;
                puttyOptions.EnableCompression = this.checkBoxCompression.Checked;
                puttyOptions.SessionName = this.cmbSessionName.Text;
            }
        }

        private void InitializeComponent()
        {
            this.cmbSessionName = new System.Windows.Forms.ComboBox();
            this.labelSession = new System.Windows.Forms.Label();
            this.checkBoxX11Forwarding = new System.Windows.Forms.CheckBox();
            this.checkBoxCompression = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmbSessionName
            // 
            this.cmbSessionName.FormattingEnabled = true;
            this.cmbSessionName.Location = new System.Drawing.Point(53, 41);
            this.cmbSessionName.Name = "cmbSessionName";
            this.cmbSessionName.Size = new System.Drawing.Size(165, 21);
            this.cmbSessionName.TabIndex = 0;
            // 
            // labelSession
            // 
            this.labelSession.AutoSize = true;
            this.labelSession.Location = new System.Drawing.Point(3, 44);
            this.labelSession.Name = "labelSession";
            this.labelSession.Size = new System.Drawing.Size(44, 13);
            this.labelSession.TabIndex = 1;
            this.labelSession.Text = "Session";
            // 
            // checkBoxX11Forwarding
            // 
            this.checkBoxX11Forwarding.AutoSize = true;
            this.checkBoxX11Forwarding.Location = new System.Drawing.Point(6, 83);
            this.checkBoxX11Forwarding.Name = "checkBoxX11Forwarding";
            this.checkBoxX11Forwarding.Size = new System.Drawing.Size(97, 17);
            this.checkBoxX11Forwarding.TabIndex = 2;
            this.checkBoxX11Forwarding.Text = "X11 forwarding";
            this.checkBoxX11Forwarding.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompression
            // 
            this.checkBoxCompression.AutoSize = true;
            this.checkBoxCompression.Location = new System.Drawing.Point(6, 106);
            this.checkBoxCompression.Name = "checkBoxCompression";
            this.checkBoxCompression.Size = new System.Drawing.Size(121, 17);
            this.checkBoxCompression.TabIndex = 3;
            this.checkBoxCompression.Text = "Enable compression";
            this.checkBoxCompression.UseVisualStyleBackColor = true;
            // 
            // PuttyOptionsControl
            // 
            this.Controls.Add(this.checkBoxCompression);
            this.Controls.Add(this.checkBoxX11Forwarding);
            this.Controls.Add(this.labelSession);
            this.Controls.Add(this.cmbSessionName);
            this.Name = "PuttyOptionsControl";
            this.Size = new System.Drawing.Size(650, 443);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
