using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class AmazonOptionPanel : UserControl, IOptionPanel
    {
        private const String AMAZON_MESSAGETITLE = "Amazon S3 Backup";

        private readonly Settings settings = Settings.Instance;

        /// <summary>
        /// Gets or sets the bucket name into/from the text box (it is a proxy).
        /// Returns text filled into associated text box.
        /// </summary>
        private String BucketName
        {
            get
            {
                return this.BucketNameTextBox.Text;
            }
            set
            {
                this.BucketNameTextBox.Text = value;
            }
        }

        public AmazonOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.AmazonBackupCheckbox.Checked = settings.UseAmazon;
            this.AccessKeyTextbox.Text = settings.AmazonAccessKey;
            this.SecretKeyTextbox.Text = settings.AmazonSecretKey;
            this.BucketName = settings.AmazonBucketName;

            UpdateAmazonControlsEnabledState();
        }

        public void SaveSettings()
        {
            settings.UseAmazon = this.AmazonBackupCheckbox.Checked;
            settings.AmazonAccessKey = this.AccessKeyTextbox.Text;
            settings.AmazonSecretKey = this.SecretKeyTextbox.Text;
            settings.AmazonBucketName = this.BucketName;
        }

        private void AmazonBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAmazonControlsEnabledState();
        }

        private void UpdateAmazonControlsEnabledState()
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
            Exception testError = null;
            using (AmazonS3 client = CreateClient())
            {
                testError = EnsureBucketExists(client);
            }

            this.ShowActionResult(testError, "Test was successful!");
            this.Cursor = Cursors.Default;
        }

        private void ShowActionResult(Exception testError, string successMessage)
        {
            if (testError == null)
            {
                this.ErrorLabel.Text = successMessage;
                this.ErrorLabel.ForeColor = Color.Black;
            }
            else
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = testError.Message;
            }
        }

        /// <summary>
        /// Ceateates new S3 webservice client. Note, that the client is Disposable
        /// </summary>
        private AmazonS3 CreateClient()
        {
            return AWSClientFactory.CreateAmazonS3Client(
                this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to upload your current configuration?",
                AMAZON_MESSAGETITLE, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (AmazonS3 client = CreateClient())
                {
                    EnsureBucketExists(client);
                    BackUpToAmazon(client);
                }
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore your current configuration?",
                AMAZON_MESSAGETITLE, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (AmazonS3 client = CreateClient())
                {
                    RestoreFromAmazon(client);
                }
            }
        }

        private Exception EnsureBucketExists(AmazonS3 client)
        {
            try
            {
                S3Bucket bucket = GetBucket(client);
                if (bucket == null)
                {
                    CreateBucket(client);
                }

                return null;
            }
            catch (Exception exception)
            {
                Logging.Error("Amazon S3 exception occured", exception);
                return exception;
            }
        }

        private S3Bucket GetBucket(AmazonS3 client)
        {
            ListBucketsRequest listRequest = new ListBucketsRequest();
            ListBucketsResponse response = client.ListBuckets(listRequest);
            return response.Buckets
                .Where(candidate => candidate.BucketName == this.BucketName)
                .FirstOrDefault();
        }

        private void CreateBucket(AmazonS3 client)
        {
            PutBucketRequest request = new PutBucketRequest();
            request.BucketName = this.BucketName;
            client.PutBucket(request);
        }

        private void BackUpToAmazon(AmazonS3 client)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest();
                request.WithBucketName(this.BucketName).WithKey(FileLocations.CONFIG_FILENAME)
                    .WithFilePath(settings.FileLocations.Configuration);

                client.PutObject(request);

                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The backup was a success!";
            }
            catch (Exception exception)
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = exception.Message;
            }
        }

        private void RestoreFromAmazon(AmazonS3 client)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest()
                    .WithBucketName(this.BucketName)
                    .WithKey(FileLocations.CONFIG_FILENAME);

                using (GetObjectResponse response = client.GetObject(request))
                {
                    response.WriteResponseStreamToFile(settings.FileLocations.Configuration);
                    settings.ForceReload();
                }

                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The restore was a success!";
            }
            catch (Exception exc)
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = exc.Message;
            }
        }
    }
}
