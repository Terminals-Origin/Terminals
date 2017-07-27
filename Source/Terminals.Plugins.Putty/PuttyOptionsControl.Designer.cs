using System.Windows.Forms;

namespace Terminals.Plugins.Putty
{
    partial class PuttyOptionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        private Label labelSession;
        private ComboBox cmbSessionName;

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
            this.components = new System.ComponentModel.Container();
            this.cmbSessionName = new System.Windows.Forms.ComboBox();
            this.labelSession = new System.Windows.Forms.Label();
            this.checkBoxVerbose = new System.Windows.Forms.CheckBox();
            this.editSessinsButton = new System.Windows.Forms.Button();
            this.editToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.refreshButton = new System.Windows.Forms.Button();
            this.sessionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sessionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSessionName
            // 
            this.cmbSessionName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSessionName.FormattingEnabled = true;
            this.cmbSessionName.Location = new System.Drawing.Point(107, 27);
            this.cmbSessionName.Name = "cmbSessionName";
            this.cmbSessionName.Size = new System.Drawing.Size(229, 21);
            this.cmbSessionName.TabIndex = 0;
            // 
            // labelSession
            // 
            this.labelSession.AutoSize = true;
            this.labelSession.Location = new System.Drawing.Point(13, 30);
            this.labelSession.Name = "labelSession";
            this.labelSession.Size = new System.Drawing.Size(72, 13);
            this.labelSession.TabIndex = 1;
            this.labelSession.Text = "Putty session:";
            // 
            // checkBoxVerbose
            // 
            this.checkBoxVerbose.AutoSize = true;
            this.checkBoxVerbose.Location = new System.Drawing.Point(107, 57);
            this.checkBoxVerbose.Name = "checkBoxVerbose";
            this.checkBoxVerbose.Size = new System.Drawing.Size(65, 17);
            this.checkBoxVerbose.TabIndex = 5;
            this.checkBoxVerbose.Text = "Verbose";
            this.checkBoxVerbose.UseVisualStyleBackColor = true;
            // 
            // editSessinsButton
            // 
            this.editSessinsButton.Location = new System.Drawing.Point(376, 26);
            this.editSessinsButton.Name = "editSessinsButton";
            this.editSessinsButton.Size = new System.Drawing.Size(50, 23);
            this.editSessinsButton.TabIndex = 6;
            this.editSessinsButton.Text = "Edit";
            this.editToolTip.SetToolTip(this.editSessinsButton, "Open putty to edit sessions");
            this.editSessinsButton.UseVisualStyleBackColor = true;
            this.editSessinsButton.Click += new System.EventHandler(this.EditSessinsButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Image = global::Terminals.Plugins.Putty.Properties.Resources.Refresh;
            this.refreshButton.Location = new System.Drawing.Point(342, 26);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(27, 23);
            this.refreshButton.TabIndex = 7;
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // PuttyOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.editSessinsButton);
            this.Controls.Add(this.checkBoxVerbose);
            this.Controls.Add(this.labelSession);
            this.Controls.Add(this.cmbSessionName);
            this.Name = "PuttyOptionsControl";
            this.Size = new System.Drawing.Size(650, 443);
            ((System.ComponentModel.ISupportInitialize)(this.sessionsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private CheckBox checkBoxVerbose;
        private Button editSessinsButton;
        private ToolTip editToolTip;
        private Button refreshButton;
        private BindingSource sessionsBindingSource;
    }
}
