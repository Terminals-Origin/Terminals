using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Forms
{
	/// <summary>
	/// Summary description for WizardDialog.
	/// </summary>
	public class WizardDialog : System.Windows.Forms.Form
	{
	    public enum TitleModes
	    {
	        None,
	        WizardPageTitle,
	        WizardPageSubTitle,
	        WizardPageCaptionTitle,
	        Steps
	    }
	
	    protected string _cachedTitle;
	    protected TitleModes _titleMode;
        protected WizardControl wizardControl;
	
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            // Initialize properties
            ResetTitleMode();
            
            // Hook into wizard page collection changes
            wizardControl.WizardPages.Cleared += new Collections.CollectionClear(OnPagesCleared);
            wizardControl.WizardPages.Inserted += new Collections.CollectionChange(OnPagesChanged);
            wizardControl.WizardPages.Removed += new Collections.CollectionChange(OnPagesChanged);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WizardDialog));
            this.wizardControl = new Crownwood.Magic.Controls.WizardControl();
            this.SuspendLayout();
            // 
            // wizardControl
            // 
            this.wizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl.Name = "wizardControl";
            this.wizardControl.Picture = ((System.Drawing.Bitmap)(resources.GetObject("wizardControl.Picture")));
            this.wizardControl.SelectedIndex = -1;
            this.wizardControl.Size = new System.Drawing.Size(416, 285);
            this.wizardControl.TabIndex = 0;
            this.wizardControl.CancelClick += new System.EventHandler(this.OnCancelClick);
            this.wizardControl.CloseClick += new System.EventHandler(this.OnCloseClick);
            this.wizardControl.NextClick += new System.ComponentModel.CancelEventHandler(this.OnNextClick);
            this.wizardControl.FinishClick += new System.EventHandler(this.OnFinishClick);
            this.wizardControl.WizardPageEnter += new Crownwood.Magic.Controls.WizardControl.WizardPageHandler(this.OnWizardPageEnter);
            this.wizardControl.BackClick += new System.ComponentModel.CancelEventHandler(this.OnBackClick);
            this.wizardControl.HelpClick += new System.EventHandler(this.OnHelpClick);
            this.wizardControl.WizardPageLeave += new Crownwood.Magic.Controls.WizardControl.WizardPageHandler(this.OnWizardPageLeave);
            this.wizardControl.WizardCaptionTitleChanged += new System.EventHandler(this.OnWizardCaptionTitleChanged);
            this.wizardControl.SelectionChanged += new System.EventHandler(this.OnSelectionChanged);
            this.wizardControl.UpdateClick += new System.EventHandler(this.OnUpdateClick);
            // 
            // WizardDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(416, 285);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.wizardControl});
            this.Name = "WizardDialog";
            this.Text = "Wizard Dialog";
            this.ResumeLayout(false);

        }
		#endregion

        public new string Text
        {
            get { return _cachedTitle; }
            
            set 
            {
                // Store the provided value
                _cachedTitle = value;
                
                // Apply the title mode extra to the end
                ApplyTitleMode();
            }
        }
        
        [Category("Wizard")]
        [Description("Determine how the title is automatically defined")]
        [DefaultValue(typeof(TitleModes), "WizardPageCaptionTitle")]
        public TitleModes TitleMode
        {
            get { return _titleMode; }
            
            set
            {
                if (_titleMode != value)
                {
                    _titleMode = value;
                    ApplyTitleMode();
                }
            }
        }

        public void ResetTitleMode()
        {
            TitleMode = TitleModes.WizardPageCaptionTitle;
        }
        
        protected virtual void ApplyTitleMode()
        {
            string newTitle = _cachedTitle;

            // Calculate new title text
            switch(_titleMode)
            {
                case TitleModes.None:
                    // Do nothing!
                    break;
                case TitleModes.Steps:
                    // Get the current page
                    int selectedPage = wizardControl.SelectedIndex;
                    int totalPages = wizardControl.WizardPages.Count;

                    // Only need separator if some text is already present                    
                    if (newTitle.Length > 0)
                        newTitle += " - ";
                    
                    // Append the required text
                    newTitle += "Step " + (selectedPage + 1).ToString() + " of " + totalPages.ToString();
                    break;
                case TitleModes.WizardPageTitle:
                    // Do we have a valid page currently selected?
                    if (wizardControl.SelectedIndex != -1)
                    {
                        // Get the page
                        WizardPage wp = wizardControl.WizardPages[wizardControl.SelectedIndex];

                        // Only need separator if some text is already present                    
                        if (newTitle.Length > 0)
                            newTitle += " - ";
                        
                        // Append page title to the title
                        newTitle += wp.Title;
                    }
                    break;
                case TitleModes.WizardPageSubTitle:
                    // Do we have a valid page currently selected?
                    if (wizardControl.SelectedIndex != -1)
                    {
                        // Get the page
                        WizardPage wp = wizardControl.WizardPages[wizardControl.SelectedIndex];
                        
                        // Only need separator if some text is already present                    
                        if (newTitle.Length > 0)
                            newTitle += " - ";

                        // Append page sub-title to the title
                        newTitle += wp.SubTitle;
                    }
                    break;
                case TitleModes.WizardPageCaptionTitle:
                    // Do we have a valid page currently selected?
                    if (wizardControl.SelectedIndex != -1)
                    {
                        // Get the page
                        WizardPage wp = wizardControl.WizardPages[wizardControl.SelectedIndex];
                        
                        // Only need separator if some text is already present                    
                        if (newTitle.Length > 0)
                            newTitle += " - ";

                        // Append page sub-title to the title
                        newTitle += wp.CaptionTitle;
                    }
                    break;
            }
            
            // Use base class to update actual caption bar
            base.Text = newTitle;
        }
        
        protected virtual void OnPagesCleared()
        {
            // Update the caption bar to reflect change in selection
            ApplyTitleMode();
        }
        
        protected virtual void OnPagesChanged(int index, object value)
        {
            // Update the caption bar to reflect change in selection
            ApplyTitleMode();
        }

        protected virtual void OnCloseClick(object sender, EventArgs e)
        {
            // By default we close the Form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual void OnFinishClick(object sender, EventArgs e)
        {
            // By default we close the Form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual void OnCancelClick(object sender, EventArgs e)
        {
            // By default we close the Form
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected virtual void OnNextClick(object sender, CancelEventArgs e)
        {
            // By default we do nothing, let derived class override
        }

        protected virtual void OnBackClick(object sender, CancelEventArgs e)
        {
            // By default we do nothing, let derived class override
        }
    
        protected virtual void OnUpdateClick(object sender, EventArgs e)
        {
            // By default we do nothing, let derived class override
        }

        protected virtual void OnHelpClick(object sender, EventArgs e)
        {
            // By default we do nothing, let derived class override
        }

        protected virtual void OnSelectionChanged(object sender, EventArgs e)
        {
            // Update the caption bar to reflect change in selection
            ApplyTitleMode();
        }

        protected virtual void OnWizardPageEnter(Controls.WizardPage wp, Controls.WizardControl wc)
        {
            // By default we do nothing, let derived class override
        }

        protected virtual void OnWizardPageLeave(Controls.WizardPage wp, Controls.WizardControl wc)
        {
            // By default we do nothing, let derived class override
        }

        protected virtual void OnWizardCaptionTitleChanged(object sender, EventArgs e)
        {
            // Update the caption bar to reflect change in selection
            ApplyTitleMode();
        }
    }
}
