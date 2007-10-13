namespace Terminals.Wizard
{
    partial class AddExistingRDPConnections
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
            if(disposing && (components != null))
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.ConnectionsCountLabel = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PendingRequestsLabel = new System.Windows.Forms.Label();
            this.PendingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 56);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please wait while Automatic Discovery is running.  Once this is complete this win" +
                "dow will close automatically.";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 151);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(464, 12);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(321, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Automatic Discovery is running.  To stop this, just hit cancel below.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(249, 26);
            this.label3.TabIndex = 4;
            this.label3.Text = "Welcome to Terminals";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Discovered Connections:";
            // 
            // ConnectionsCountLabel
            // 
            this.ConnectionsCountLabel.AutoSize = true;
            this.ConnectionsCountLabel.Location = new System.Drawing.Point(141, 225);
            this.ConnectionsCountLabel.Name = "ConnectionsCountLabel";
            this.ConnectionsCountLabel.Size = new System.Drawing.Size(13, 13);
            this.ConnectionsCountLabel.TabIndex = 6;
            this.ConnectionsCountLabel.Text = "0";
            this.ConnectionsCountLabel.Click += new System.EventHandler(this.ConnectionsCountLabel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(160, 192);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(216, 88);
            this.dataGridView1.TabIndex = 7;
            // 
            // PendingRequestsLabel
            // 
            this.PendingRequestsLabel.AutoSize = true;
            this.PendingRequestsLabel.Location = new System.Drawing.Point(141, 247);
            this.PendingRequestsLabel.Name = "PendingRequestsLabel";
            this.PendingRequestsLabel.Size = new System.Drawing.Size(13, 13);
            this.PendingRequestsLabel.TabIndex = 9;
            this.PendingRequestsLabel.Text = "0";
            // 
            // PendingLabel
            // 
            this.PendingLabel.AutoSize = true;
            this.PendingLabel.Location = new System.Drawing.Point(37, 247);
            this.PendingLabel.Name = "PendingLabel";
            this.PendingLabel.Size = new System.Drawing.Size(97, 13);
            this.PendingLabel.TabIndex = 8;
            this.PendingLabel.Text = "Pending Requests:";
            // 
            // AddExistingRDPConnections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.PendingRequestsLabel);
            this.Controls.Add(this.PendingLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ConnectionsCountLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Name = "AddExistingRDPConnections";
            this.Size = new System.Drawing.Size(379, 283);
            this.Load += new System.EventHandler(this.AddExistingRDPConnections_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label ConnectionsCountLabel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label PendingRequestsLabel;
        private System.Windows.Forms.Label PendingLabel;
    }
}
