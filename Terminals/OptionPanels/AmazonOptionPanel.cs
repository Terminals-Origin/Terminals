using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Forms
{
    internal class AmazonOptionPanel : OptionDialogCategoryPanel
    {
        private Panel panel1;
        private GroupBox groupBox4;
        private Label label22;
        private TextBox BucketNameTextBox;
        private Button RestoreButton;
        private Button BackupButton;
        private Label ErrorLabel;
        private Button TestButton;
        private Label label17;
        private Label label16;
        private TextBox SecretKeyTextbox;
        private TextBox AccessKeyTextbox;
        private CheckBox AmazonBackupCheckbox;
        private PictureBox pictureBox1;

        private readonly String amazonBucketName = "Terminals";
        private readonly String amazonConfigKeyName = "Terminals.config";

        public AmazonOptionPanel()
        {
            InitializeComponent();
        }

        #region InitializeComponent
        
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.BucketNameTextBox = new System.Windows.Forms.TextBox();
            this.RestoreButton = new System.Windows.Forms.Button();
            this.BackupButton = new System.Windows.Forms.Button();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.TestButton = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.SecretKeyTextbox = new System.Windows.Forms.TextBox();
            this.AccessKeyTextbox = new System.Windows.Forms.TextBox();
            this.AmazonBackupCheckbox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 332);
            this.panel1.TabIndex = 25;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.BucketNameTextBox);
            this.groupBox4.Controls.Add(this.RestoreButton);
            this.groupBox4.Controls.Add(this.BackupButton);
            this.groupBox4.Controls.Add(this.ErrorLabel);
            this.groupBox4.Controls.Add(this.TestButton);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.SecretKeyTextbox);
            this.groupBox4.Controls.Add(this.AccessKeyTextbox);
            this.groupBox4.Controls.Add(this.AmazonBackupCheckbox);
            this.groupBox4.Controls.Add(this.pictureBox1);
            this.groupBox4.Location = new System.Drawing.Point(6, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(500, 216);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 100);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(72, 13);
            this.label22.TabIndex = 23;
            this.label22.Text = "Bucket Name";
            // 
            // BucketNameTextBox
            // 
            this.BucketNameTextBox.Location = new System.Drawing.Point(143, 97);
            this.BucketNameTextBox.Name = "BucketNameTextBox";
            this.BucketNameTextBox.Size = new System.Drawing.Size(212, 20);
            this.BucketNameTextBox.TabIndex = 22;
            // 
            // RestoreButton
            // 
            this.RestoreButton.Location = new System.Drawing.Point(87, 184);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(75, 23);
            this.RestoreButton.TabIndex = 21;
            this.RestoreButton.Text = "Restore";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // BackupButton
            // 
            this.BackupButton.Location = new System.Drawing.Point(6, 185);
            this.BackupButton.Name = "BackupButton";
            this.BackupButton.Size = new System.Drawing.Size(75, 23);
            this.BackupButton.TabIndex = 20;
            this.BackupButton.Text = "Backup";
            this.BackupButton.UseVisualStyleBackColor = true;
            this.BackupButton.Click += new System.EventHandler(this.BackupButton_Click);
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(3, 137);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(211, 70);
            this.ErrorLabel.TabIndex = 19;
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(280, 124);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(75, 23);
            this.TestButton.TabIndex = 18;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(125, 13);
            this.label17.TabIndex = 17;
            this.label17.Text = "Your Secret Access Key:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(105, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = "Your Access Key ID:";
            // 
            // SecretKeyTextbox
            // 
            this.SecretKeyTextbox.Location = new System.Drawing.Point(143, 70);
            this.SecretKeyTextbox.Name = "SecretKeyTextbox";
            this.SecretKeyTextbox.Size = new System.Drawing.Size(212, 20);
            this.SecretKeyTextbox.TabIndex = 15;
            // 
            // AccessKeyTextbox
            // 
            this.AccessKeyTextbox.Location = new System.Drawing.Point(143, 43);
            this.AccessKeyTextbox.Name = "AccessKeyTextbox";
            this.AccessKeyTextbox.Size = new System.Drawing.Size(212, 20);
            this.AccessKeyTextbox.TabIndex = 14;
            // 
            // AmazonBackupCheckbox
            // 
            this.AmazonBackupCheckbox.AutoSize = true;
            this.AmazonBackupCheckbox.Location = new System.Drawing.Point(6, 20);
            this.AmazonBackupCheckbox.Name = "AmazonBackupCheckbox";
            this.AmazonBackupCheckbox.Size = new System.Drawing.Size(222, 17);
            this.AmazonBackupCheckbox.TabIndex = 13;
            this.AmazonBackupCheckbox.Text = "Backup Favorites to Amazons S3 Service";
            this.AmazonBackupCheckbox.UseVisualStyleBackColor = true;
            this.AmazonBackupCheckbox.CheckedChanged += new System.EventHandler(this.AmazonBackupCheckbox_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Terminals.Properties.Resources.amazon;
            this.pictureBox1.Location = new System.Drawing.Point(220, 153);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(135, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // AmazonOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "AmazonOptionPanel";
            this.Size = new System.Drawing.Size(513, 332);
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public override void Init()
        {
            this.AmazonBackupCheckbox.Checked = Settings.UseAmazon;
            this.AccessKeyTextbox.Text = Settings.AmazonAccessKey;
            this.SecretKeyTextbox.Text = Settings.AmazonSecretKey;

            this.AccessKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.SecretKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BucketNameTextBox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.TestButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BackupButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.RestoreButton.Enabled = this.AmazonBackupCheckbox.Checked;
        }

        public override Boolean Save()
        {
            try
            {
                Settings.DelayConfigurationSave = true;

                this.AmazonBackupCheckbox.Checked = Settings.UseAmazon;
                this.AccessKeyTextbox.Text = Settings.AmazonAccessKey;
                this.SecretKeyTextbox.Text = Settings.AmazonSecretKey;

                return true;
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error(ex);
                return false;
            }
        }

        private void AmazonBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.AccessKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.SecretKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BucketNameTextBox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.TestButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BackupButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.RestoreButton.Enabled = this.AmazonBackupCheckbox.Checked;
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
                String testBucket = Guid.NewGuid().ToString();
                wrapper.AddBucket(testBucket);
                String bucket = wrapper.ListBucket(testBucket);
                wrapper.DeleteBucket(testBucket);

                try
                {
                    String terminals = wrapper.ListBucket(this.amazonBucketName);
                }
                catch (Exception exc)
                {
                    if (exc.Message == "The specified bucket does not exist")
                    {
                        wrapper.AddBucket(this.amazonBucketName);
                        String terminals = wrapper.ListBucket(this.amazonBucketName);
                    }
                }

                this.ErrorLabel.Text = "Test was successful!";
                this.ErrorLabel.ForeColor = Color.Black;
            }
            catch (Exception exc)
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = exc.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to upload your current configuration?", "Amazon S3 Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
                String url = String.Empty;
                try
                {
                    String terminals = wrapper.ListBucket(this.amazonBucketName);
                }
                catch (Exception)
                {
                    wrapper.AddBucket(this.amazonBucketName);
                }

                try
                {
                    wrapper.AddFileObject(this.amazonBucketName, this.amazonConfigKeyName, Terminals.Program.ConfigurationFileLocation);
                    url = wrapper.GetUrl(this.amazonBucketName, this.amazonConfigKeyName);
                }
                catch (Exception exc)
                {
                    this.ErrorLabel.ForeColor = Color.Red;
                    this.ErrorLabel.Text = exc.Message;
                    return;
                }

                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The backup was a success!";
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore your current configuration?", "Amazon S3 Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
                try
                {
                    String terminals = wrapper.ListBucket(this.amazonBucketName);
                }
                catch (Exception exc)
                {
                    this.ErrorLabel.ForeColor = Color.Red;
                    this.ErrorLabel.Text = exc.Message;
                    return;
                }

                try
                {
                    wrapper.GetFileObject(this.amazonBucketName, this.amazonConfigKeyName, Terminals.Program.ConfigurationFileLocation);
                }
                catch (Exception exc)
                {
                    this.ErrorLabel.ForeColor = Color.Red;
                    this.ErrorLabel.Text = exc.Message;
                    return;
                }

                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The restore was a success!";
            }
        }
    }
}
