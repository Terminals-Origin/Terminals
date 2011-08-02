using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using FlickrNet;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal class FlickrOptionPanel : OptionDialogCategoryPanel
    {
        private Panel panel1;
        private GroupBox groupBox9;
        private Label label25;
        private Label lblText2;
        private Label lblText1;
        private Button CompleteAuthButton;
        private Button AuthorizeFlickrButton;
        private Label label8;

        private String tempFrob;

        public FlickrOptionPanel()
        {
            InitializeComponent();
        }

        #region InitializeComponent
        
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.lblText2 = new System.Windows.Forms.Label();
            this.lblText1 = new System.Windows.Forms.Label();
            this.CompleteAuthButton = new System.Windows.Forms.Button();
            this.AuthorizeFlickrButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 331);
            this.panel1.TabIndex = 26;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label25);
            this.groupBox9.Controls.Add(this.lblText2);
            this.groupBox9.Controls.Add(this.lblText1);
            this.groupBox9.Controls.Add(this.CompleteAuthButton);
            this.groupBox9.Controls.Add(this.AuthorizeFlickrButton);
            this.groupBox9.Controls.Add(this.label8);
            this.groupBox9.Location = new System.Drawing.Point(6, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(500, 213);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(6, 17);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(382, 40);
            this.label25.TabIndex = 21;
            this.label25.Text = "Use Flickr as storage place for screen captures.";
            // 
            // lblText2
            // 
            this.lblText2.Location = new System.Drawing.Point(87, 133);
            this.lblText2.Name = "label10";
            this.lblText2.Size = new System.Drawing.Size(301, 66);
            this.lblText2.TabIndex = 20;
            this.lblText2.Text = "Second you must click the Complete button to finish the process. Only do this AFTER " +
                                    "you have accepted Terminals access to your account on the Flickr Web Site.";
            // 
            // lblText1
            // 
            this.lblText1.Location = new System.Drawing.Point(87, 57);
            this.lblText1.Name = "label9";
            this.lblText1.Size = new System.Drawing.Size(301, 61);
            this.lblText1.TabIndex = 19;
            this.lblText1.Text = "First you must first Authorize Terminals with your Flickr account. Press the Authorize " +
                                    "button now, login to your Flickr Account and allow Terminals limited access to your account.";
            // 
            // CompleteAuthButton
            // 
            this.CompleteAuthButton.Enabled = false;
            this.CompleteAuthButton.Location = new System.Drawing.Point(6, 133);
            this.CompleteAuthButton.Name = "CompleteAuthButton";
            this.CompleteAuthButton.Size = new System.Drawing.Size(75, 23);
            this.CompleteAuthButton.TabIndex = 18;
            this.CompleteAuthButton.Text = "Complete...";
            this.CompleteAuthButton.UseVisualStyleBackColor = true;
            this.CompleteAuthButton.Click += new EventHandler(CompleteAuthButton_Click);
            // 
            // AuthorizeFlickrButton
            // 
            this.AuthorizeFlickrButton.Location = new System.Drawing.Point(9, 57);
            this.AuthorizeFlickrButton.Name = "AuthorizeFlickrButton";
            this.AuthorizeFlickrButton.Size = new System.Drawing.Size(75, 23);
            this.AuthorizeFlickrButton.TabIndex = 17;
            this.AuthorizeFlickrButton.Text = "Authorize...";
            this.AuthorizeFlickrButton.UseVisualStyleBackColor = true;
            this.AuthorizeFlickrButton.Click += new EventHandler(AuthorizeFlickrButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 16;
            // 
            // FlickrOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "FlickrOptionPanel";
            this.Size = new System.Drawing.Size(514, 331);
            this.panel1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public override void Init()
        {
        }

        public override Boolean Save()
        {
            return true;
        }

        private void AuthorizeFlickrButton_Click(object sender, EventArgs e)
        {
            // Create Flickr instance    
            Flickr flickr = new Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);
            // Get Frob        
            this.tempFrob = flickr.AuthGetFrob();
            // Calculate the URL at Flickr to redirect the user to    
            String flickrUrl = flickr.AuthCalcUrl(this.tempFrob, AuthLevel.Write);
            // The following line will load the URL in the users default browser.    
            Process.Start(flickrUrl);
            CompleteAuthButton.Enabled = true;
        }

        private void CompleteAuthButton_Click(object sender, EventArgs e)
        {
            // Create Flickr instance
            Flickr flickr = new Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);
            try
            {
                // use the temporary Frob to get the authentication
                Auth auth = flickr.AuthGetToken(this.tempFrob);

                // Store this Token for later usage,
                // or set your Flickr instance to use it.
                MessageBox.Show("User authenticated successfully");
                Logging.Log.Info(String.Format("User authenticated successfully. Authentication token is {0}. User id is {1}, username is {2}", auth.Token, auth.User.UserId, auth.User.Username));
                flickr.AuthToken = auth.Token;
                Settings.FlickrToken = auth.Token;
            }
            catch (FlickrException ex)
            {
                // If user did not authenticat your application
                // then a FlickrException will be thrown.
                Logging.Log.Info("User not authenticated successfully", ex);
                MessageBox.Show("User did not authenticate you" + ex.Message);
            }
        }
    }
}
