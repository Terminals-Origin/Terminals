namespace Terminals {
    partial class PopupTerminal {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabControl1 = new TabControl.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.ShowToolTipOnTitle = false;
            this.tabControl1.Size = new System.Drawing.Size(445, 266);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.DoubleClick += new System.EventHandler(this.tabControl1_DoubleClick);
            this.tabControl1.TabControlItemClosing += new TabControl.TabControlItemClosingHandler(this.tabControl1_TabControlItemClosing);
            this.tabControl1.MouseHover += new System.EventHandler(this.tcTerminals_MouseHover);
            this.tabControl1.MouseLeave += new System.EventHandler(this.tcTerminals_MouseLeave);
            // 
            // PopupTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 266);
            this.Controls.Add(this.tabControl1);
            this.Name = "PopupTerminal";
            this.Text = "Terminal Window";
            this.Load += new System.EventHandler(this.PopupTerminal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl.TabControl tabControl1;
    }
}