namespace Terminals.Network
{
    partial class NetworkShares
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.wmiServerCredentials1 = new Terminals.Network.WMI.WMIServerCredentials();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 83);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(399, 135);
            this.dataGridView1.TabIndex = 0;
            // 
            // wmiServerCredentials1
            // 
            this.wmiServerCredentials1.Dock = System.Windows.Forms.DockStyle.Top;
            this.wmiServerCredentials1.Location = new System.Drawing.Point(0, 0);
            this.wmiServerCredentials1.Name = "wmiServerCredentials1";
            this.wmiServerCredentials1.Password = "Rob";
            this.wmiServerCredentials1.SelectedServer = "";
            this.wmiServerCredentials1.Size = new System.Drawing.Size(399, 83);
            this.wmiServerCredentials1.TabIndex = 1;
            this.wmiServerCredentials1.Username = "\\APOLLO\\Rob";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(194, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NetworkShares
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.wmiServerCredentials1);
            this.Name = "NetworkShares";
            this.Size = new System.Drawing.Size(399, 218);
            this.Load += new System.EventHandler(this.NetworkShares_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private Terminals.Network.WMI.WMIServerCredentials wmiServerCredentials1;
        private System.Windows.Forms.Button button1;
    }
}
