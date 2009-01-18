namespace Terminals
{
    partial class KeyGenForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.labelAlgorithm = new System.Windows.Forms.Label();
        	this.labelBitCount = new System.Windows.Forms.Label();
        	this.labelTag = new System.Windows.Forms.Label();
        	this.textBoxTag = new System.Windows.Forms.TextBox();
        	this.progressBarGenerate = new System.Windows.Forms.ProgressBar();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.buttonSave = new System.Windows.Forms.Button();
        	this.buttonGenerate = new System.Windows.Forms.Button();
        	this.algorithmBox = new System.Windows.Forms.ComboBox();
        	this.bitCountBox = new System.Windows.Forms.ComboBox();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.labelRandomness = new System.Windows.Forms.Label();
        	this.publicKeyBox = new System.Windows.Forms.TextBox();
        	this.labelpublicKey = new System.Windows.Forms.Label();
        	this.fingerprintBox = new System.Windows.Forms.TextBox();
        	this.labelfingerprint = new System.Windows.Forms.Label();
        	this.groupBox1.SuspendLayout();
        	this.groupBox2.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// labelAlgorithm
        	// 
        	this.labelAlgorithm.AutoSize = true;
        	this.labelAlgorithm.Location = new System.Drawing.Point(19, 28);
        	this.labelAlgorithm.Name = "labelAlgorithm";
        	this.labelAlgorithm.Size = new System.Drawing.Size(50, 13);
        	this.labelAlgorithm.TabIndex = 0;
        	this.labelAlgorithm.Text = "Algorithm";
        	this.labelAlgorithm.UseWaitCursor = true;
        	// 
        	// labelBitCount
        	// 
        	this.labelBitCount.AutoSize = true;
        	this.labelBitCount.Location = new System.Drawing.Point(233, 28);
        	this.labelBitCount.Name = "labelBitCount";
        	this.labelBitCount.Size = new System.Drawing.Size(49, 13);
        	this.labelBitCount.TabIndex = 1;
        	this.labelBitCount.Text = "Bit count";
        	this.labelBitCount.UseWaitCursor = true;
        	// 
        	// labelTag
        	// 
        	this.labelTag.AutoSize = true;
        	this.labelTag.Location = new System.Drawing.Point(19, 166);
        	this.labelTag.Name = "labelTag";
        	this.labelTag.Size = new System.Drawing.Size(71, 13);
        	this.labelTag.TabIndex = 2;
        	this.labelTag.Text = "Key comment";
        	this.labelTag.UseWaitCursor = true;
        	// 
        	// textBoxTag
        	// 
        	this.textBoxTag.Location = new System.Drawing.Point(135, 163);
        	this.textBoxTag.MaxLength = 32;
        	this.textBoxTag.Name = "textBoxTag";
        	this.textBoxTag.Size = new System.Drawing.Size(257, 20);
        	this.textBoxTag.TabIndex = 3;
        	this.textBoxTag.TextChanged += new System.EventHandler(this.textBoxTag_TextChanged);
        	// 
        	// progressBarGenerate
        	// 
        	this.progressBarGenerate.Location = new System.Drawing.Point(22, 38);
        	this.progressBarGenerate.Name = "progressBarGenerate";
        	this.progressBarGenerate.Size = new System.Drawing.Size(370, 23);
        	this.progressBarGenerate.TabIndex = 99;
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Location = new System.Drawing.Point(15, 308);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 5;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// buttonSave
        	// 
        	this.buttonSave.Location = new System.Drawing.Point(344, 308);
        	this.buttonSave.Name = "buttonSave";
        	this.buttonSave.Size = new System.Drawing.Size(75, 23);
        	this.buttonSave.TabIndex = 6;
        	this.buttonSave.Text = "Save";
        	this.buttonSave.UseVisualStyleBackColor = true;
        	this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
        	// 
        	// buttonGenerate
        	// 
        	this.buttonGenerate.Location = new System.Drawing.Point(168, 308);
        	this.buttonGenerate.Name = "buttonGenerate";
        	this.buttonGenerate.Size = new System.Drawing.Size(108, 23);
        	this.buttonGenerate.TabIndex = 4;
        	this.buttonGenerate.Text = "Generate Now";
        	this.buttonGenerate.UseVisualStyleBackColor = true;
        	this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
        	// 
        	// algorithmBox
        	// 
        	this.algorithmBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.algorithmBox.FormattingEnabled = true;
        	this.algorithmBox.Items.AddRange(new object[] {
        	        	        	"DSA",
        	        	        	"RSA"});
        	this.algorithmBox.Location = new System.Drawing.Point(84, 25);
        	this.algorithmBox.Name = "algorithmBox";
        	this.algorithmBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
        	this.algorithmBox.Size = new System.Drawing.Size(103, 21);
        	this.algorithmBox.TabIndex = 1;
        	// 
        	// bitCountBox
        	// 
        	this.bitCountBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.bitCountBox.FormattingEnabled = true;
        	this.bitCountBox.Items.AddRange(new object[] {
        	        	        	"768",
        	        	        	"1024",
        	        	        	"2048"});
        	this.bitCountBox.Location = new System.Drawing.Point(289, 25);
        	this.bitCountBox.Name = "bitCountBox";
        	this.bitCountBox.Size = new System.Drawing.Size(103, 21);
        	this.bitCountBox.TabIndex = 2;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.algorithmBox);
        	this.groupBox1.Controls.Add(this.labelAlgorithm);
        	this.groupBox1.Controls.Add(this.bitCountBox);
        	this.groupBox1.Controls.Add(this.labelBitCount);
        	this.groupBox1.Location = new System.Drawing.Point(15, 214);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(404, 76);
        	this.groupBox1.TabIndex = 101;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Parameters";
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.labelRandomness);
        	this.groupBox2.Controls.Add(this.publicKeyBox);
        	this.groupBox2.Controls.Add(this.labelpublicKey);
        	this.groupBox2.Controls.Add(this.fingerprintBox);
        	this.groupBox2.Controls.Add(this.labelfingerprint);
        	this.groupBox2.Controls.Add(this.textBoxTag);
        	this.groupBox2.Controls.Add(this.progressBarGenerate);
        	this.groupBox2.Controls.Add(this.labelTag);
        	this.groupBox2.Location = new System.Drawing.Point(15, 19);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(404, 189);
        	this.groupBox2.TabIndex = 102;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Key";
        	// 
        	// labelRandomness
        	// 
        	this.labelRandomness.AutoSize = true;
        	this.labelRandomness.Location = new System.Drawing.Point(19, 22);
        	this.labelRandomness.Name = "labelRandomness";
        	this.labelRandomness.Size = new System.Drawing.Size(373, 13);
        	this.labelRandomness.TabIndex = 104;
        	this.labelRandomness.Text = "Please generate some randomness by moving the mouse over the blank area.";
        	// 
        	// publicKeyBox
        	// 
        	this.publicKeyBox.Location = new System.Drawing.Point(22, 38);
        	this.publicKeyBox.Multiline = true;
        	this.publicKeyBox.Name = "publicKeyBox";
        	this.publicKeyBox.ReadOnly = true;
        	this.publicKeyBox.Size = new System.Drawing.Size(370, 93);
        	this.publicKeyBox.TabIndex = 103;
        	// 
        	// labelpublicKey
        	// 
        	this.labelpublicKey.AutoSize = true;
        	this.labelpublicKey.Location = new System.Drawing.Point(19, 22);
        	this.labelpublicKey.Name = "labelpublicKey";
        	this.labelpublicKey.Size = new System.Drawing.Size(279, 13);
        	this.labelpublicKey.TabIndex = 102;
        	this.labelpublicKey.Text = "public key for pasting into OpensSSH authorized_keys file";
        	// 
        	// fingerprintBox
        	// 
        	this.fingerprintBox.Location = new System.Drawing.Point(135, 137);
        	this.fingerprintBox.Name = "fingerprintBox";
        	this.fingerprintBox.ReadOnly = true;
        	this.fingerprintBox.Size = new System.Drawing.Size(257, 20);
        	this.fingerprintBox.TabIndex = 101;
        	// 
        	// labelfingerprint
        	// 
        	this.labelfingerprint.AutoSize = true;
        	this.labelfingerprint.Location = new System.Drawing.Point(19, 140);
        	this.labelfingerprint.Name = "labelfingerprint";
        	this.labelfingerprint.Size = new System.Drawing.Size(56, 13);
        	this.labelfingerprint.TabIndex = 100;
        	this.labelfingerprint.Text = "finger print";
        	// 
        	// KeyGenForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(438, 346);
        	this.Controls.Add(this.groupBox2);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.buttonGenerate);
        	this.Controls.Add(this.buttonSave);
        	this.Controls.Add(this.buttonCancel);
        	this.Name = "KeyGenForm";
        	this.Text = "KeyGenForm";
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	this.groupBox2.ResumeLayout(false);
        	this.groupBox2.PerformLayout();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label labelAlgorithm;
        private System.Windows.Forms.Label labelBitCount;
        private System.Windows.Forms.Label labelTag;
        private System.Windows.Forms.TextBox textBoxTag;
        private System.Windows.Forms.ProgressBar progressBarGenerate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.ComboBox algorithmBox;
        private System.Windows.Forms.ComboBox bitCountBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelpublicKey;
        private System.Windows.Forms.TextBox fingerprintBox;
        private System.Windows.Forms.Label labelfingerprint;
        private System.Windows.Forms.Label labelRandomness;
        private System.Windows.Forms.TextBox publicKeyBox;
    }
}