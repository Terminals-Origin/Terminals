using System;
using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class ConnectCommandOptionPanel
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtInitialDirectory = new System.Windows.Forms.TextBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.lblInitialDirectory = new System.Windows.Forms.Label();
            this.chkExecuteBeforeConnect = new System.Windows.Forms.CheckBox();
            this.lblArguments = new System.Windows.Forms.Label();
            this.chkWaitForExit = new System.Windows.Forms.CheckBox();
            this.lblCommand = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 334);
            this.panel1.TabIndex = 26;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtInitialDirectory);
            this.groupBox5.Controls.Add(this.txtArguments);
            this.groupBox5.Controls.Add(this.txtCommand);
            this.groupBox5.Controls.Add(this.lblInitialDirectory);
            this.groupBox5.Controls.Add(this.chkExecuteBeforeConnect);
            this.groupBox5.Controls.Add(this.lblArguments);
            this.groupBox5.Controls.Add(this.chkWaitForExit);
            this.groupBox5.Controls.Add(this.lblCommand);
            this.groupBox5.Location = new System.Drawing.Point(6, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(500, 150);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.Location = new System.Drawing.Point(102, 97);
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Size = new System.Drawing.Size(265, 20);
            this.txtInitialDirectory.TabIndex = 22;
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(102, 70);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(265, 20);
            this.txtArguments.TabIndex = 21;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(102, 43);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(265, 20);
            this.txtCommand.TabIndex = 20;
            // 
            // lblInitialDirectory
            // 
            this.lblInitialDirectory.AutoSize = true;
            this.lblInitialDirectory.Location = new System.Drawing.Point(3, 100);
            this.lblInitialDirectory.Name = "label13";
            this.lblInitialDirectory.Size = new System.Drawing.Size(79, 13);
            this.lblInitialDirectory.TabIndex = 26;
            this.lblInitialDirectory.Text = "Initial Directory:";
            // 
            // chkExecuteBeforeConnect
            // 
            this.chkExecuteBeforeConnect.AutoSize = true;
            this.chkExecuteBeforeConnect.Location = new System.Drawing.Point(6, 20);
            this.chkExecuteBeforeConnect.Name = "chkExecuteBeforeConnect";
            this.chkExecuteBeforeConnect.Size = new System.Drawing.Size(175, 17);
            this.chkExecuteBeforeConnect.TabIndex = 19;
            this.chkExecuteBeforeConnect.Text = "Enable execute before connect";
            this.chkExecuteBeforeConnect.UseVisualStyleBackColor = true;
            this.chkExecuteBeforeConnect.CheckedChanged += new EventHandler(chkExecuteBeforeConnect_CheckedChanged);
            // 
            // lblArguments
            // 
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(3, 73);
            this.lblArguments.Name = "label12";
            this.lblArguments.Size = new System.Drawing.Size(57, 13);
            this.lblArguments.TabIndex = 25;
            this.lblArguments.Text = "Arguments";
            // 
            // chkWaitForExit
            // 
            this.chkWaitForExit.AutoSize = true;
            this.chkWaitForExit.Location = new System.Drawing.Point(6, 124);
            this.chkWaitForExit.Name = "chkWaitForExit";
            this.chkWaitForExit.Size = new System.Drawing.Size(82, 17);
            this.chkWaitForExit.TabIndex = 24;
            this.chkWaitForExit.Text = "Wait for exit";
            this.chkWaitForExit.UseVisualStyleBackColor = true;
            // 
            // lblCommand
            // 
            this.lblCommand.AutoSize = true;
            this.lblCommand.Location = new System.Drawing.Point(3, 46);
            this.lblCommand.Name = "label11";
            this.lblCommand.Size = new System.Drawing.Size(57, 13);
            this.lblCommand.TabIndex = 23;
            this.lblCommand.Text = "Command:";
            // 
            // ConnectCommandOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "ConnectCommandOptionPanel";
            this.Size = new System.Drawing.Size(513, 334);
            this.panel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private GroupBox groupBox5;
        private TextBox txtInitialDirectory;
        private TextBox txtArguments;
        private TextBox txtCommand;
        private Label lblInitialDirectory;
        private CheckBox chkExecuteBeforeConnect;
        private Label lblArguments;
        private CheckBox chkWaitForExit;
        private Label lblCommand;
    }
}
