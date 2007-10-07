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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabbedTools1 = new Terminals.Connections.TabbedTools();
            this.tabbedTools2 = new Terminals.Connections.TabbedTools();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabbedTools1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabbedTools2);
            this.splitContainer1.Size = new System.Drawing.Size(700, 500);
            this.splitContainer1.SplitterDistance = 371;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabbedTools1
            // 
            this.tabbedTools1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedTools1.Location = new System.Drawing.Point(0, 0);
            this.tabbedTools1.Name = "tabbedTools1";
            this.tabbedTools1.Size = new System.Drawing.Size(700, 371);
            this.tabbedTools1.TabIndex = 0;
            // 
            // tabbedTools2
            // 
            this.tabbedTools2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedTools2.Location = new System.Drawing.Point(0, 0);
            this.tabbedTools2.Name = "tabbedTools2";
            this.tabbedTools2.Size = new System.Drawing.Size(700, 124);
            this.tabbedTools2.TabIndex = 0;
            // 
            // NetworkingToolsLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "NetworkingToolsLayout";
            this.Size = new System.Drawing.Size(700, 500);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private TabbedTools tabbedTools1;
        private TabbedTools tabbedTools2;

    }
}
