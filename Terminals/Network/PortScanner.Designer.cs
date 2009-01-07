namespace Terminals.Network {
    partial class PortScanner {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.ScanResultsLabel = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.TextBox();
            this.pa = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.e = new System.Windows.Forms.TextBox();
            this.d = new System.Windows.Forms.TextBox();
            this.c = new System.Windows.Forms.TextBox();
            this.b = new System.Windows.Forms.TextBox();
            this.a = new System.Windows.Forms.TextBox();
            this.resultsGridView = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ScanResultsLabel);
            this.panel1.Controls.Add(this.pb);
            this.panel1.Controls.Add(this.pa);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.StopButton);
            this.panel1.Controls.Add(this.StartButton);
            this.panel1.Controls.Add(this.e);
            this.panel1.Controls.Add(this.d);
            this.panel1.Controls.Add(this.c);
            this.panel1.Controls.Add(this.b);
            this.panel1.Controls.Add(this.a);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(485, 69);
            this.panel1.TabIndex = 0;
            // 
            // ScanResultsLabel
            // 
            this.ScanResultsLabel.AutoSize = true;
            this.ScanResultsLabel.Location = new System.Drawing.Point(345, 40);
            this.ScanResultsLabel.Name = "ScanResultsLabel";
            this.ScanResultsLabel.Size = new System.Drawing.Size(0, 13);
            this.ScanResultsLabel.TabIndex = 11;
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(209, 35);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(27, 20);
            this.pb.TabIndex = 6;
            this.pb.Text = "10";
            this.pb.KeyUp += new System.Windows.Forms.KeyEventHandler(this.pb_KeyUp);
            // 
            // pa
            // 
            this.pa.Location = new System.Drawing.Point(209, 9);
            this.pa.Name = "pa";
            this.pa.Size = new System.Drawing.Size(27, 20);
            this.pa.TabIndex = 5;
            this.pa.Text = "1";
            this.pa.KeyUp += new System.Windows.Forms.KeyEventHandler(this.pa_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Addresses:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(169, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ports:";
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(255, 35);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 8;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(255, 9);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 7;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // e
            // 
            this.e.Location = new System.Drawing.Point(136, 35);
            this.e.MaxLength = 3;
            this.e.Name = "e";
            this.e.Size = new System.Drawing.Size(27, 20);
            this.e.TabIndex = 4;
            this.e.Text = "10";
            this.e.KeyUp += new System.Windows.Forms.KeyEventHandler(this.e_KeyUp);
            // 
            // d
            // 
            this.d.Location = new System.Drawing.Point(136, 9);
            this.d.MaxLength = 3;
            this.d.Name = "d";
            this.d.Size = new System.Drawing.Size(27, 20);
            this.d.TabIndex = 3;
            this.d.Text = "1";
            this.d.KeyUp += new System.Windows.Forms.KeyEventHandler(this.d_KeyUp);
            // 
            // c
            // 
            this.c.Location = new System.Drawing.Point(103, 22);
            this.c.MaxLength = 3;
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(27, 20);
            this.c.TabIndex = 2;
            this.c.Text = "0";
            this.c.KeyUp += new System.Windows.Forms.KeyEventHandler(this.c_KeyUp);
            // 
            // b
            // 
            this.b.Location = new System.Drawing.Point(70, 22);
            this.b.MaxLength = 3;
            this.b.Name = "b";
            this.b.Size = new System.Drawing.Size(27, 20);
            this.b.TabIndex = 1;
            this.b.Text = "0";
            this.b.KeyUp += new System.Windows.Forms.KeyEventHandler(this.b_KeyUp);
            // 
            // a
            // 
            this.a.Location = new System.Drawing.Point(37, 22);
            this.a.MaxLength = 3;
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(27, 20);
            this.a.TabIndex = 0;
            this.a.Text = "10";
            this.a.KeyUp += new System.Windows.Forms.KeyEventHandler(this.a_KeyUp);
            // 
            // resultsGridView
            // 
            this.resultsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsGridView.Location = new System.Drawing.Point(0, 69);
            this.resultsGridView.Name = "resultsGridView";
            this.resultsGridView.Size = new System.Drawing.Size(485, 225);
            this.resultsGridView.TabIndex = 1;
            // 
            // PortScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultsGridView);
            this.Controls.Add(this.panel1);
            this.Name = "PortScanner";
            this.Size = new System.Drawing.Size(485, 294);
            this.Load += new System.EventHandler(this.PortScanner_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView resultsGridView;
        private System.Windows.Forms.TextBox e;
        private System.Windows.Forms.TextBox d;
        private System.Windows.Forms.TextBox c;
        private System.Windows.Forms.TextBox b;
        private System.Windows.Forms.TextBox a;
        private System.Windows.Forms.TextBox pb;
        private System.Windows.Forms.TextBox pa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label ScanResultsLabel;
    }
}
