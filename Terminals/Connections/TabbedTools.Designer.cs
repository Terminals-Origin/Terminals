namespace Terminals.Connections
{
    partial class TabbedTools
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ping1 = new Metro.Ping();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.traceRoute1 = new Metro.TraceRoute();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(523, 335);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ping1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(515, 309);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Ping";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ping1
            // 
            this.ping1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ping1.Location = new System.Drawing.Point(3, 3);
            this.ping1.Name = "ping1";
            this.ping1.Size = new System.Drawing.Size(509, 303);
            this.ping1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.traceRoute1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(515, 309);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Trace Route";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // traceRoute1
            // 
            this.traceRoute1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.traceRoute1.Location = new System.Drawing.Point(3, 3);
            this.traceRoute1.Name = "traceRoute1";
            this.traceRoute1.Size = new System.Drawing.Size(509, 303);
            this.traceRoute1.TabIndex = 0;
            // 
            // TabbedTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "TabbedTools";
            this.Size = new System.Drawing.Size(523, 335);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Metro.Ping ping1;
        private System.Windows.Forms.TabPage tabPage2;
        private Metro.TraceRoute traceRoute1;
    }
}
