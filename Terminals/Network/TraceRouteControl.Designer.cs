namespace Terminals.Network
{
    partial class TraceRouteControl
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ZGraph = new ZedGraph.ZedGraphControl();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextHost = new System.Windows.Forms.TextBox();
            this.ResolveCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(511, 255);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(503, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Raw Results";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(497, 223);
            this.dataGridView1.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ZGraph);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(503, 229);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ZGraph
            // 
            this.ZGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZGraph.Location = new System.Drawing.Point(3, 3);
            this.ZGraph.Name = "ZGraph";
            this.ZGraph.ScrollGrace = 0D;
            this.ZGraph.ScrollMaxX = 0D;
            this.ZGraph.ScrollMaxY = 0D;
            this.ZGraph.ScrollMaxY2 = 0D;
            this.ZGraph.ScrollMinX = 0D;
            this.ZGraph.ScrollMinY = 0D;
            this.ZGraph.ScrollMinY2 = 0D;
            this.ZGraph.Size = new System.Drawing.Size(497, 223);
            this.ZGraph.TabIndex = 0;
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(273, 2);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 2;
            this.ButtonStop.Text = "Sto&p";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(192, 2);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(75, 23);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "&Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Host:";
            // 
            // TextHost
            // 
            this.TextHost.Location = new System.Drawing.Point(41, 4);
            this.TextHost.Name = "TextHost";
            this.TextHost.Size = new System.Drawing.Size(145, 20);
            this.TextHost.TabIndex = 0;
            this.TextHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextHost_KeyPress);
            // 
            // ResolveCheckBox
            // 
            this.ResolveCheckBox.AutoSize = true;
            this.ResolveCheckBox.Checked = true;
            this.ResolveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ResolveCheckBox.Location = new System.Drawing.Point(376, 7);
            this.ResolveCheckBox.Name = "ResolveCheckBox";
            this.ResolveCheckBox.Size = new System.Drawing.Size(126, 17);
            this.ResolveCheckBox.TabIndex = 3;
            this.ResolveCheckBox.Text = "Resolve Host Names";
            this.ResolveCheckBox.UseVisualStyleBackColor = true;
            // 
            // TraceRouteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ResolveCheckBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextHost);
            this.Name = "TraceRouteControl";
            this.Size = new System.Drawing.Size(517, 285);
            this.Load += new System.EventHandler(this.TraceRoute_Load);
            this.Resize += new System.EventHandler(this.TraceRoute_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private ZedGraph.ZedGraphControl ZGraph;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextHost;
        private System.Windows.Forms.CheckBox ResolveCheckBox;
    }
}
