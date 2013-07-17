namespace Terminals.Connections
{
    partial class NetworkingToolsLayout
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
            this.tabbedTools1 = new Terminals.Connections.TabbedTools();
            this.SuspendLayout();
            // 
            // tabbedTools1
            // 
            this.tabbedTools1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedTools1.Location = new System.Drawing.Point(0, 0);
            this.tabbedTools1.Name = "tabbedTools1";
            this.tabbedTools1.Size = new System.Drawing.Size(808, 500);
            this.tabbedTools1.TabIndex = 1;
            this.tabbedTools1.Load += new System.EventHandler(this.TabbedTools1_Load);
            // 
            // NetworkingToolsLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabbedTools1);
            this.Name = "NetworkingToolsLayout";
            this.Size = new System.Drawing.Size(808, 500);
            this.ResumeLayout(false);

        }

        #endregion

        private TabbedTools tabbedTools1;


    }
}
