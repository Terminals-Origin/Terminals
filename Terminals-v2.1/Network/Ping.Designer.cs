namespace Terminals.Network
{
    partial class Ping
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
            this.TextHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabResults = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TabGraph = new System.Windows.Forms.TabPage();
            this.ZGraph = new ZedGraph.ZedGraphControl();
            this.label2 = new System.Windows.Forms.Label();
            this.DelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.TabResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.TabGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // TextHost
            // 
            this.TextHost.Location = new System.Drawing.Point(41, 3);
            this.TextHost.Name = "TextHost";
            this.TextHost.Size = new System.Drawing.Size(145, 20);
            this.TextHost.TabIndex = 0;
            this.TextHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextHost_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host:";
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(192, 1);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(75, 23);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "&Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(273, 1);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 2;
            this.ButtonStop.Text = "Sto&p";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.TabResults);
            this.tabControl1.Controls.Add(this.TabGraph);
            this.tabControl1.Location = new System.Drawing.Point(3, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(491, 260);
            this.tabControl1.TabIndex = 5;
            // 
            // TabResults
            // 
            this.TabResults.Controls.Add(this.dataGridView1);
            this.TabResults.Location = new System.Drawing.Point(4, 22);
            this.TabResults.Name = "TabResults";
            this.TabResults.Padding = new System.Windows.Forms.Padding(3);
            this.TabResults.Size = new System.Drawing.Size(483, 234);
            this.TabResults.TabIndex = 0;
            this.TabResults.Text = "Raw Results";
            this.TabResults.UseVisualStyleBackColor = true;
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
            this.dataGridView1.Size = new System.Drawing.Size(477, 228);
            this.dataGridView1.TabIndex = 5;
            // 
            // TabGraph
            // 
            this.TabGraph.Controls.Add(this.ZGraph);
            this.TabGraph.Location = new System.Drawing.Point(4, 22);
            this.TabGraph.Name = "TabGraph";
            this.TabGraph.Padding = new System.Windows.Forms.Padding(3);
            this.TabGraph.Size = new System.Drawing.Size(483, 234);
            this.TabGraph.TabIndex = 1;
            this.TabGraph.Text = "Graph";
            this.TabGraph.UseVisualStyleBackColor = true;
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
            this.ZGraph.Size = new System.Drawing.Size(477, 228);
            this.ZGraph.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(376, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Delay:";
            // 
            // DelayNumericUpDown
            // 
            this.DelayNumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.DelayNumericUpDown.Location = new System.Drawing.Point(419, 4);
            this.DelayNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DelayNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.DelayNumericUpDown.Name = "DelayNumericUpDown";
            this.DelayNumericUpDown.ReadOnly = true;
            this.DelayNumericUpDown.Size = new System.Drawing.Size(71, 20);
            this.DelayNumericUpDown.TabIndex = 3;
            this.DelayNumericUpDown.ThousandsSeparator = true;
            this.DelayNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // Ping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DelayNumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextHost);
            this.Name = "Ping";
            this.Size = new System.Drawing.Size(497, 289);
            this.Load += new System.EventHandler(this.Ping_Load);
            this.Resize += new System.EventHandler(this.Ping_Resize);
            this.tabControl1.ResumeLayout(false);
            this.TabResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.TabGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabResults;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage TabGraph;
        private ZedGraph.ZedGraphControl ZGraph;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown DelayNumericUpDown;
    }
}
