using System;
using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class CaptureOptionPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.chkAutoSwitchToCaptureCheckbox = new System.Windows.Forms.CheckBox();
            this.chkEnableCaptureToFolder = new System.Windows.Forms.CheckBox();
            this.chkEnableCaptureToClipboard = new System.Windows.Forms.CheckBox();
            this.ButtonBrowseCaptureFolder = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.txtScreenCaptureFolder = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 333);
            this.panel1.TabIndex = 26;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.chkAutoSwitchToCaptureCheckbox);
            this.groupBox8.Controls.Add(this.chkEnableCaptureToFolder);
            this.groupBox8.Controls.Add(this.chkEnableCaptureToClipboard);
            this.groupBox8.Controls.Add(this.ButtonBrowseCaptureFolder);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Controls.Add(this.txtScreenCaptureFolder);
            this.groupBox8.Location = new System.Drawing.Point(6, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(500, 138);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            // 
            // chkAutoSwitchToCaptureCheckbox
            // 
            this.chkAutoSwitchToCaptureCheckbox.AutoSize = true;
            this.chkAutoSwitchToCaptureCheckbox.Checked = true;
            this.chkAutoSwitchToCaptureCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSwitchToCaptureCheckbox.Location = new System.Drawing.Point(26, 66);
            this.chkAutoSwitchToCaptureCheckbox.Name = "chkAutoSwitchToCaptureCheckbox";
            this.chkAutoSwitchToCaptureCheckbox.Size = new System.Drawing.Size(191, 17);
            this.chkAutoSwitchToCaptureCheckbox.TabIndex = 27;
            this.chkAutoSwitchToCaptureCheckbox.Text = "Auto switch to manager on capture";
            this.chkAutoSwitchToCaptureCheckbox.UseVisualStyleBackColor = true;
            // 
            // chkEnableCaptureToFolder
            // 
            this.chkEnableCaptureToFolder.AutoSize = true;
            this.chkEnableCaptureToFolder.Location = new System.Drawing.Point(6, 43);
            this.chkEnableCaptureToFolder.Name = "chkEnableCaptureToFolder";
            this.chkEnableCaptureToFolder.Size = new System.Drawing.Size(174, 17);
            this.chkEnableCaptureToFolder.TabIndex = 26;
            this.chkEnableCaptureToFolder.Text = "Enable screen capture to folder";
            this.chkEnableCaptureToFolder.UseVisualStyleBackColor = true;
            this.chkEnableCaptureToFolder.CheckedChanged += new EventHandler(chkEnableCaptureToFolder_CheckedChanged);
            // 
            // chkEnableCaptureToClipboard
            // 
            this.chkEnableCaptureToClipboard.AutoSize = true;
            this.chkEnableCaptureToClipboard.Checked = true;
            this.chkEnableCaptureToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableCaptureToClipboard.Location = new System.Drawing.Point(6, 20);
            this.chkEnableCaptureToClipboard.Name = "chkEnableCaptureToClipboard";
            this.chkEnableCaptureToClipboard.Size = new System.Drawing.Size(191, 17);
            this.chkEnableCaptureToClipboard.TabIndex = 25;
            this.chkEnableCaptureToClipboard.Text = "Enable screen capture to clipboard";
            this.chkEnableCaptureToClipboard.UseVisualStyleBackColor = true;
            // 
            // ButtonBrowseCaptureFolder
            // 
            this.ButtonBrowseCaptureFolder.Location = new System.Drawing.Point(383, 105);
            this.ButtonBrowseCaptureFolder.Name = "ButtonBrowseCaptureFolder";
            this.ButtonBrowseCaptureFolder.Size = new System.Drawing.Size(65, 23);
            this.ButtonBrowseCaptureFolder.TabIndex = 24;
            this.ButtonBrowseCaptureFolder.Text = "Browse...";
            this.ButtonBrowseCaptureFolder.UseVisualStyleBackColor = true;
            this.ButtonBrowseCaptureFolder.Click += new EventHandler(ButtonBrowseCaptureFolder_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 91);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(133, 13);
            this.label23.TabIndex = 23;
            this.label23.Text = "Screen capture root folder:";
            // 
            // txtScreenCaptureFolder
            // 
            this.txtScreenCaptureFolder.Location = new System.Drawing.Point(6, 107);
            this.txtScreenCaptureFolder.Name = "txtScreenCaptureFolder";
            this.txtScreenCaptureFolder.Size = new System.Drawing.Size(371, 20);
            this.txtScreenCaptureFolder.TabIndex = 22;
            // 
            // CaptureOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "CaptureOptionPanel";
            this.Size = new System.Drawing.Size(513, 333);
            this.panel1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private GroupBox groupBox8;
        private CheckBox chkAutoSwitchToCaptureCheckbox;
        private CheckBox chkEnableCaptureToFolder;
        private CheckBox chkEnableCaptureToClipboard;
        private Button ButtonBrowseCaptureFolder;
        private Label label23;
        private TextBox txtScreenCaptureFolder;
    }
}
