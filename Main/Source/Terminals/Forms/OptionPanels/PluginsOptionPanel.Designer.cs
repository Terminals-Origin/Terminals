namespace Terminals.Forms.OptionPanels
{
    partial class PluginsOptionPanel
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
            this.components = new System.ComponentModel.Container();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.pluginsListbox = new System.Windows.Forms.CheckedListBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(15, 14);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(262, 39);
            this.descriptionLabel.TabIndex = 1;
            this.descriptionLabel.Text = "Select plugins you want to use.\r\nTo apply you need restart the application.\r\nFor " +
    "disabled plugins, related protocol favorites are lost.";
            // 
            // pluginsListbox
            // 
            this.pluginsListbox.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.pluginsListbox, System.Windows.Forms.ErrorIconAlignment.TopRight);
            this.pluginsListbox.Location = new System.Drawing.Point(18, 65);
            this.pluginsListbox.Name = "pluginsListbox";
            this.pluginsListbox.Size = new System.Drawing.Size(332, 124);
            this.pluginsListbox.TabIndex = 2;
            this.pluginsListbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.PluginsListbox_ItemCheck);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // PluginsOptionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pluginsListbox);
            this.Controls.Add(this.descriptionLabel);
            this.Name = "PluginsOptionPanel";
            this.Size = new System.Drawing.Size(402, 300);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.CheckedListBox pluginsListbox;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
