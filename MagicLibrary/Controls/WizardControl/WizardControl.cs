// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Controls
{
    [ToolboxBitmap(typeof(WizardControl))]
    [DefaultProperty("Profile")]
    [Designer(typeof(WizardControlDesigner))]
    public class WizardControl : UserControl
	{
	    // Define enumerations and structures
	    public enum Status
	    {
	        Default,
	        Yes,
	        No
	    }
	    
	    public enum Profiles
	    {
	        Install,
	        Configure,
	        Controller
	    }
	    
	    // Class wide constants
	    protected const int _panelGap = 10;
	    protected const int _buttonGap = 10;
	    protected static Image _standardPicture;
	
	    // Instance fields
        protected Image _picture;
        protected string _title;
        protected Font _fontTitle;
        protected Font _fontSubTitle;
        protected Color _colorTitle;
        protected Color _colorSubTitle;
        protected Profiles _profile;
        protected bool _assignDefault;
        protected WizardPage _selectedPage;
        protected Status _showUpdate, _enableUpdate;
        protected Status _showCancel, _enableCancel;
        protected Status _showBack, _enableBack;
        protected Status _showNext, _enableNext;
        protected Status _showFinish, _enableFinish;
        protected Status _showClose, _enableClose;
        protected Status _showHelp, _enableHelp;
        protected WizardPageCollection _wizardPages;
	    
	    // Instance designer fields
        protected System.Windows.Forms.Panel _panelTop;
        protected System.Windows.Forms.Panel _panelBottom;
        protected System.Windows.Forms.Button _buttonUpdate;
        protected System.Windows.Forms.Button _buttonCancel;
        protected System.Windows.Forms.Button _buttonBack;
        protected System.Windows.Forms.Button _buttonNext;
        protected System.Windows.Forms.Button _buttonFinish;
        protected System.Windows.Forms.Button _buttonClose;
        protected System.Windows.Forms.Button _buttonHelp;
        protected Crownwood.Magic.Controls.TabControl _tabControl;
		protected System.ComponentModel.Container components = null;

        // Delegate definitions
        public delegate void WizardPageHandler(WizardPage wp, WizardControl wc);

        // Instance events
        public event WizardPageHandler WizardPageEnter;
        public event WizardPageHandler WizardPageLeave;
        public event EventHandler WizardCaptionTitleChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler UpdateClick;
        public event EventHandler CancelClick;
        public event EventHandler FinishClick;
        public event EventHandler CloseClick;
        public event EventHandler HelpClick;
        public event CancelEventHandler NextClick;
        public event CancelEventHandler BackClick;

        static WizardControl()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _standardPicture = ResourceHelper.LoadBitmap(Type.GetType("Crownwood.Magic.Controls.WizardControl"),
                                                         "Crownwood.Magic.Resources.WizardPicture.bmp");
        }

		public WizardControl()
		{
			InitializeComponent();
			
			// No page currently selected
			_selectedPage = null;
			
	        // Hook into tab control events
	        _tabControl.SelectionChanged += new EventHandler(OnTabSelectionChanged);

            // Create our collection of wizard pages
            _wizardPages = new WizardPageCollection();
            
            // Hook into collection events
            _wizardPages.Cleared += new Collections.CollectionClear(OnWizardCleared);
            _wizardPages.Inserted += new Collections.CollectionChange(OnWizardInserted);
            _wizardPages.Removed += new Collections.CollectionChange(OnWizardRemoved);

            // Hook into drawing events
            _panelTop.Resize += new EventHandler(OnRepaintPanels);
            _panelTop.Paint += new PaintEventHandler(OnPaintTopPanel);
            _panelBottom.Resize += new EventHandler(OnRepaintPanels);
            _panelBottom.Paint += new PaintEventHandler(OnPaintBottomPanel);

            // Initialize state
            _showUpdate = _enableUpdate = Status.Default;
            _showCancel = _enableUpdate = Status.Default;
            _showBack = _enableBack = Status.Default;
            _showNext = _enableNext = Status.Default;
            _showFinish = _enableFinish = Status.Default;
            _showClose = _enableClose = Status.Default;
            _showHelp = _enableHelp = Status.Default;
            
            // Default properties
            ResetProfile();
            ResetTitle();
            ResetTitleFont();
            ResetTitleColor();
            ResetSubTitleFont();
            ResetSubTitleColor();
            ResetPicture();
            ResetAssignDefaultButton();
            
            // Position and enable/disable control button state
            UpdateControlButtons();
   		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._tabControl = new Crownwood.Magic.Controls.TabControl();
            this._panelTop = new System.Windows.Forms.Panel();
            this._panelBottom = new System.Windows.Forms.Panel();
            this._buttonUpdate = new System.Windows.Forms.Button();
            this._buttonBack = new System.Windows.Forms.Button();
            this._buttonNext = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._buttonFinish = new System.Windows.Forms.Button();
            this._buttonClose = new System.Windows.Forms.Button();
            this._buttonHelp = new System.Windows.Forms.Button();
            this._panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right);
            this._tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
            this._tabControl.IDEPixelBorder = false;
            this._tabControl.Location = new System.Drawing.Point(0, 80);
            this._tabControl.Multiline = true;
            this._tabControl.MultilineFullWidth = true;
            this._tabControl.Name = "_tabControl";
            this._tabControl.ShowArrows = false;
            this._tabControl.ShowClose = false;
            this._tabControl.Size = new System.Drawing.Size(424, 264);
            this._tabControl.TabIndex = 0;
            // 
            // _panelTop
            // 
            this._panelTop.BackColor = System.Drawing.SystemColors.Window;
            this._panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelTop.Name = "_panelTop";
            this._panelTop.Size = new System.Drawing.Size(424, 80);
            this._panelTop.TabIndex = 1;
            // 
            // _panelBottom
            // 
            this._panelBottom.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                       this._buttonUpdate,
                                                                                       this._buttonBack,
                                                                                       this._buttonNext,
                                                                                       this._buttonCancel,
                                                                                       this._buttonFinish,
                                                                                       this._buttonClose,
                                                                                       this._buttonHelp});
            this._panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panelBottom.Location = new System.Drawing.Point(0, 344);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(424, 48);
            this._panelBottom.TabIndex = 2;
            // 
            // _buttonUpdate
            // 
            this._buttonUpdate.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonUpdate.Location = new System.Drawing.Point(8, 14);
            this._buttonUpdate.Name = "_buttonUpdate";
            this._buttonUpdate.TabIndex = 4;
            this._buttonUpdate.Text = "Update";
            this._buttonUpdate.Click += new System.EventHandler(this.OnButtonUpdate);
            // 
            // _buttonBack
            // 
            this._buttonBack.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonBack.Location = new System.Drawing.Point(56, 14);
            this._buttonBack.Name = "_buttonBack";
            this._buttonBack.TabIndex = 3;
            this._buttonBack.Text = "< Back";
            this._buttonBack.Click += new System.EventHandler(this.OnButtonBack);
            // 
            // _buttonNext
            // 
            this._buttonNext.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonNext.Location = new System.Drawing.Point(120, 14);
            this._buttonNext.Name = "_buttonNext";
            this._buttonNext.TabIndex = 2;
            this._buttonNext.Text = "Next >";
            this._buttonNext.Click += new System.EventHandler(this.OnButtonNext);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonCancel.Location = new System.Drawing.Point(184, 14);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.TabIndex = 1;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.Click += new System.EventHandler(this.OnButtonCancel);
            // 
            // _buttonFinish
            // 
            this._buttonFinish.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonFinish.Location = new System.Drawing.Point(248, 14);
            this._buttonFinish.Name = "_buttonFinish";
            this._buttonFinish.TabIndex = 0;
            this._buttonFinish.Text = "Finish";
            this._buttonFinish.Click += new System.EventHandler(this.OnButtonFinish);
            // 
            // _buttonClose
            // 
            this._buttonClose.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonClose.Location = new System.Drawing.Point(304, 14);
            this._buttonClose.Name = "_buttonClose";
            this._buttonClose.TabIndex = 0;
            this._buttonClose.Text = "Close";
            this._buttonClose.Click += new System.EventHandler(this.OnButtonClose);
            // 
            // _buttonHelp
            // 
            this._buttonHelp.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this._buttonHelp.Location = new System.Drawing.Point(360, 14);
            this._buttonHelp.Name = "_buttonHelp";
            this._buttonHelp.TabIndex = 0;
            this._buttonHelp.Text = "Help";
            this._buttonHelp.Click += new System.EventHandler(this.OnButtonHelp);
            // 
            // WizardControl
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this._tabControl,
                                                                          this._panelTop,
                                                                          this._panelBottom});
            this.Name = "WizardControl";
            this.Size = new System.Drawing.Size(424, 392);
            this._panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
		
        [Category("Wizard")]
        [Description("Access to underlying TabControl instance")]
        public Controls.TabControl TabControl
        {
            get { return _tabControl; }
        }

        [Category("Wizard")]
        [Description("Access to underlying header panel")]
        public Panel HeaderPanel
        {
            get { return _panelTop; }
        }

        [Category("Wizard")]
        [Description("Access to underlying trailer panel")]
        public Panel TrailerPanel
        {
            get { return _panelBottom; }
        }

        [Category("Wizard")]
        [Description("Collection of wizard pages")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageCollection WizardPages
		{
		    get { return _wizardPages; }
		}
		
        [Category("Wizard")]
        [Description("Determine default operation of buttons")]
        [DefaultValue(typeof(Profiles), "Configure")]
        public Profiles Profile
        {
            get { return _profile; }
		    
            set 
            {
                if (_profile != value)
                {
                    _profile = value;
		            
                    switch(_profile)
                    {
                        case Profiles.Install:
                        case Profiles.Configure:
                            // Current page selection determines if full page is needed
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Get the selected wizard page
                                WizardPage wp = _wizardPages[_tabControl.SelectedIndex];
                
                                // Should we be presented in full page?
                                if (wp.FullPage)
                                    EnterFullPage();
                                else
                                {
                                    // Controller profile is not allowed to be outside of FullMode
                                    if (_profile != Profiles.Controller)
                                        LeaveFullPage();
                                }
                            }
                            else
                                LeaveFullPage();
                            
                            _tabControl.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.HideAlways; 
                            break;
                        case Profiles.Controller:
                            // Controller is always full page
                            EnterFullPage();
                                
                            _tabControl.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.ShowAlways;
                            break;
                    }
		            
                    // Position and enable/disable control button state
                    UpdateControlButtons();
                }
            }
        }

        public void ResetProfile()
        {
            Profile = Profiles.Configure;
        }
        
        [Category("Wizard")]
        [Description("Main title text")]
        public Image Picture
        {
            get { return _picture; }
            
            set
            {
                _picture = value;
                _panelTop.Invalidate();
            }
        }

        protected bool ShouldSerializePicture()
        {
            return _picture.Equals(_standardPicture);
        }
        
        public void ResetPicture()
        {
            Picture = _standardPicture;
        }
        
        [Category("Wizard")]
		[Description("Main title text")]
		[Localizable(true)]
		public string Title
		{
		    get { return _title; }
		    
		    set
		    {
		        _title = value;
		        _panelTop.Invalidate();
		    }
		}
		
        public void ResetTitle()
        {
            Title = "Welcome to the Wizard Control";
        }

        protected bool ShouldSerializeTitle()
        {
            return !_title.Equals("Welcome to the Wizard Control");
        }
    
        [Category("Wizard")]
		[Description("Font for drawing main title text")]
		public Font TitleFont
		{
		    get { return _fontTitle; }
		    
		    set
		    {
		        _fontTitle = value;
		        _panelTop.Invalidate();
		    }
		}
		
        public void ResetTitleFont()
        {
            TitleFont = new Font("Tahoma", 10, FontStyle.Bold);
        }

        protected bool ShouldSerializeTitleFont()
        {
            return !_fontTitle.Equals(new Font("Tahoma", 10, FontStyle.Bold));
        }
    
        [Category("Wizard")]
        [Description("Font for drawing main sub-title text")]
        public Font SubTitleFont
        {
            get { return _fontSubTitle; }
		    
            set
            {
                _fontSubTitle = value;
                _panelTop.Invalidate();
            }
        }
		
        public void ResetSubTitleFont()
        {
            _fontSubTitle = new Font("Tahoma", 8, FontStyle.Regular);
        }

        protected bool ShouldSerializeSubTitleFont()
        {
            return !_fontSubTitle.Equals(new Font("Tahoma", 8, FontStyle.Regular));
        }

        [Category("Wizard")]
        [Description("Color for drawing main title text")]
        public Color TitleColor
		{
		    get { return _colorTitle; }
		    
		    set
		    {
		        _colorTitle = value;
		        _panelTop.Invalidate();
		    }
		}

		public void ResetTitleColor()
		{
		    TitleColor = base.ForeColor;
		}

        protected bool ShouldSerializeTitleColor()
        {
            return !_colorTitle.Equals(base.ForeColor);
        }
		
        [Category("Wizard")]
        [Description("Determine is a default button should be auto-assigned")]
        [DefaultValue(false)]
        public bool AssignDefaultButton
        {
            get { return _assignDefault; }
            
            set
            {
                if (_assignDefault != value)
                {
                    _assignDefault = value;
                    AutoAssignDefaultButton();
                }
            }
        }

        public void ResetAssignDefaultButton()
        {
            AssignDefaultButton = false;
        }

        [Category("Wizard")]
        [Description("Color for drawing main sub-title text")]
        public Color SubTitleColor
        {
            get { return _colorSubTitle; }
		    
            set
            {
                _colorSubTitle = value;
                _panelTop.Invalidate();
            }
        }

        public void ResetSubTitleColor()
        {
            SubTitleColor = base.ForeColor;
        }

        protected bool ShouldSerializeSubTitleColor()
        {
            return !_colorSubTitle.Equals(base.ForeColor);
        }

        [Category("Control Buttons")]
        [Description("Define visibility of Update button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowUpdateButton
        {
            get { return _showUpdate; }
            
            set 
            { 
                if (_showUpdate != value)
                {
                    _showUpdate = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Update button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableUpdateButton
        {
            get { return _enableUpdate; }
            
            set 
            { 
                if (_enableUpdate != value)
                {
                    _enableUpdate = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Update control button")]
        [DefaultValue("Update")]
        [Localizable(true)]
        public string ButtonUpdateText
        {
            get { return _buttonUpdate.Text; }
            set { _buttonUpdate.Text = value; }
        }

        [Category("Control Buttons")]
        [Description("Define visibility of Cancel button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowCancelButton
        {
            get { return _showCancel; }
            
            set 
            { 
                if (_showCancel != value)
                {
                    _showCancel = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Cancel button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableCancelButton
        {
            get { return _enableCancel; }
            
            set 
            { 
                if (_enableCancel != value)
                {
                    _enableCancel = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Cancel control button")]
        [DefaultValue("Cancel")]
        [Localizable(true)]
        public string ButtonCancelText
        {
            get { return _buttonCancel.Text; }
            set { _buttonCancel.Text = value; }
        }

        [Category("Control Buttons")]
        [Description("Define visibility of Back button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowBackButton
        {
            get { return _showBack; }
            
            set 
            { 
                if (_showBack != value)
                {
                    _showBack = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Back button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableBackButton
        {
            get { return _enableBack; }
            
            set 
            { 
                if (_enableBack != value)
                {
                    _enableBack = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Back control button")]
        [DefaultValue("< Back")]
        [Localizable(true)]
        public string ButtonBackText
        {
            get { return _buttonBack.Text; }
            set { _buttonBack.Text = value; }
        }

        [Category("Control Buttons")]
        [Description("Define visibility of Next button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowNextButton
        {
            get { return _showNext; }
            
            set 
            { 
                if (_showNext != value)
                {
                    _showNext = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Next button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableNextButton
        {
            get { return _enableBack; }
            
            set 
            { 
                if (_enableNext != value)
                {
                    _enableNext = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Next control button")]
        [DefaultValue("Next >")]
        [Localizable(true)]
        public string ButtonNextText
        {
            get { return _buttonNext.Text; }
            set { _buttonNext.Text = value; }
        }

        [Category("Control Buttons")]
        [Description("Define visibility of Finish button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowFinishButton
        {
            get { return _showFinish; }
            
            set 
            { 
                if (_showFinish != value)
                {
                    _showFinish = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Finish button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableFinishButton
        {
            get { return _enableFinish; }
            
            set 
            { 
                if (_enableFinish != value)
                {
                    _enableFinish = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Finish control button")]
        [DefaultValue("Finish")]
        [Localizable(true)]
        public string ButtonFinishText
        {
            get { return _buttonFinish.Text; }
            set { _buttonFinish.Text = value; }
        }
        
        [Category("Control Buttons")]
        [Description("Define visibility of Close button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowCloseButton
        {
            get { return _showClose; }
            
            set 
            { 
                if (_showClose != value)
                {
                    _showClose = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Close button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableCloseButton
        {
            get { return _enableClose; }
            
            set 
            { 
                if (_enableClose != value)
                {
                    _enableClose = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Close control button")]
        [DefaultValue("Close")]
        [Localizable(true)]
        public string ButtonCloseText
        {
            get { return _buttonClose.Text; }
            set { _buttonClose.Text = value; }
        }

        [Category("Control Buttons")]
        [Description("Define visibility of Help button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowHelpButton
        {
            get { return _showHelp; }
            
            set 
            { 
                if (_showHelp != value)
                {
                    _showHelp = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Define selectability of Help button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableHelpButton
        {
            get { return _enableHelp; }
            
            set 
            { 
                if (_enableHelp != value)
                {
                    _enableHelp = value;
                    UpdateControlButtons();
                }
            }
        }

        [Category("Control Buttons")]
        [Description("Modify the text for the Help control button")]
        [DefaultValue("Help")]
        [Localizable(true)]
        public string ButtonHelpText
        {
            get { return _buttonHelp.Text; }
            set { _buttonHelp.Text = value; }
        }

        [Category("Wizard")]
        [Description("Index of currently selected WizardPage")]
        public int SelectedIndex
        {
            get { return _tabControl.SelectedIndex; }
            set { _tabControl.SelectedIndex = value; }
        }
        
        public virtual void OnWizardPageEnter(WizardPage wp)
        {
            if (WizardPageEnter != null)
                WizardPageEnter(wp, this);
        }

        public virtual void OnWizardPageLeave(WizardPage wp)
        {
            if (WizardPageLeave != null)
                WizardPageLeave(wp, this);
        }

        public virtual void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }
                
        public virtual void OnCloseClick(EventArgs e)
        {
            if (CloseClick != null)
                CloseClick(this, e);
        }

        public virtual void OnFinishClick(EventArgs e)
        {
            if (FinishClick != null)
                FinishClick(this, e);
        }
    
        public virtual void OnNextClick(CancelEventArgs e)
        {
            if (NextClick != null)
                NextClick(this, e);
        }
    
        public virtual void OnBackClick(CancelEventArgs e)
        {
            if (BackClick != null)
                BackClick(this, e);
        }

        public virtual void OnCancelClick(EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(this, e);
        }
        
        public virtual void OnUpdateClick(EventArgs e)
        {
            if (UpdateClick != null)
                UpdateClick(this, e);
        }
        
        public virtual void OnHelpClick(EventArgs e)
        {
            if (HelpClick != null)
                HelpClick(this, e);
        }

        protected void UpdateControlButtons()
        {
            // Track next button inserted position
            int x = this.Width - _buttonGap - _buttonFinish.Width;
            
            bool showHelp = ShouldShowHelp();
            bool showClose = ShouldShowClose();
            bool showFinish = ShouldShowFinish();
            bool showNext = ShouldShowNext();
            bool showBack = ShouldShowBack();
            bool showCancel = ShouldShowCancel();
            bool showUpdate = ShouldShowUpdate();
            
            if (showHelp) 
            {
                _buttonHelp.Left = x;
                x -= _buttonHelp.Width + _buttonGap;
                _buttonHelp.Enabled = ShouldEnableHelp();
                _buttonHelp.Show();
            }
            else
                _buttonHelp.Hide();

            if (showClose) 
            {
                _buttonClose.Left = x;
                x -= _buttonClose.Width + _buttonGap;
                _buttonClose.Enabled = ShouldEnableClose();
                _buttonClose.Show();
            }
            else
                _buttonClose.Hide();

            if (showFinish) 
            {
                _buttonFinish.Left = x;
                x -= _buttonFinish.Width + _buttonGap;
                _buttonFinish.Enabled = ShouldEnableFinish();
                _buttonFinish.Show();
            }
            else
                _buttonFinish.Hide();

            if (showNext) 
            {
                _buttonNext.Left = x;
                x -= _buttonNext.Width + _buttonGap;
                _buttonNext.Enabled = ShouldEnableNext();
                _buttonNext.Show();
            }
            else
                _buttonNext.Hide();

            if (showBack) 
            {
                _buttonBack.Left = x;
                x -= _buttonBack.Width + _buttonGap;
                _buttonBack.Enabled = ShouldEnableBack();
                _buttonBack.Show();
            }
            else
                _buttonBack.Hide();

            if (showCancel) 
            {
                _buttonCancel.Left = x;
                x -= _buttonCancel.Width + _buttonGap;
                _buttonCancel.Enabled = ShouldEnableCancel();
                _buttonCancel.Show();
            }
            else
                _buttonCancel.Hide();

            if (showUpdate) 
            {
                _buttonUpdate.Left = x;
                x -= _buttonUpdate.Width + _buttonGap;
                _buttonUpdate.Enabled = ShouldEnableUpdate();
                _buttonUpdate.Show();
            }
            else
                _buttonUpdate.Hide();
                
            AutoAssignDefaultButton();
        }
        
        protected void AutoAssignDefaultButton()
        {
            // Get our parent Form instance
            Form parentForm = this.FindForm();
            
            // Cannot assign a default button if we are not on a Form
            if (parentForm != null)
            {
                // Can only assign a particular button if we have been requested 
                // to auto- assign and we are on a selected page
                if (_assignDefault && (_tabControl.SelectedIndex >= 0))
                {
                    // Button default depends on the profile mode
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Is this the last page?
                            if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 1))
                            {
                                // Then use the Close button
                                parentForm.AcceptButton = _buttonClose;
                            }
                            else
                            {
                                // Is this the second from last page?
                                if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 2))
                                {
                                    // Then use the Cancel button
                                    parentForm.AcceptButton = _buttonCancel;
                                }
                                else
                                {
                                    // Then use the Next button
                                    parentForm.AcceptButton = _buttonNext;
                                }
                            }
                            break;
                        case Profiles.Configure:
                            // Is this the last page?
                            if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 1))
                            {
                                // Then always use the Finish button
                                parentForm.AcceptButton = _buttonFinish;
                            }
                            else
                            {
                                // Else we are not on last page, use the Next button
                                parentForm.AcceptButton = _buttonNext;
                            }
                            break;
                        case Profiles.Controller:
                            // Always use the Update button
                            parentForm.AcceptButton = _buttonUpdate;
                            break;
                    }
                }
                else
                {
                    // Remove any assigned default button
                    parentForm.AcceptButton = null;
                }
            }
        }
        
        protected bool ShouldShowClose()
        {
            bool ret = false;
        
            switch(_showClose)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                switch(_profile)
                {
                    case Profiles.Install:
                        // Must have at least one page
                        if (_tabControl.SelectedIndex != -1)
                        {
                            // Cannot 'Close' unless on the last page
                            if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 1))
                                ret = true;
                        }
                        break;
                    case Profiles.Configure:
                        break;
                    case Profiles.Controller:
                        break;
                }
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableClose()
        {
            bool ret = false;
        
            switch(_enableClose)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        protected bool ShouldShowFinish()
        {
            bool ret = false;
        
            switch(_showFinish)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            break;
                    }
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableFinish()
        {
            bool ret = false;
        
            switch(_enableFinish)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        protected bool ShouldShowNext()
        {
            bool ret = false;

            switch(_showNext)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Next' when at the last or second to last pages
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 2))
                                    ret = true;
                            }
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            break;
                    }
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableNext()
        {
            bool ret = false;

            switch(_enableNext)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Next' when at the last or second to last pages
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 2))
                                    ret = true;
                            }
                            break;
                        case Profiles.Configure:
                        case Profiles.Controller:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Next' when at the last or second to last pages
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 1))
                                    ret = true;
                            }
                            break;
                    }
                    break;
            }

            return ret;
        }

        protected bool ShouldShowBack()
        {
            bool ret = false;

            switch(_showBack)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Cannot 'Back' when one the first page or on the last two special pages
                            if ((_tabControl.SelectedIndex > 0) && (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 2)))
                                ret = true;
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            break;
                    }
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableBack()
        {
            bool ret = false;

            switch(_enableBack)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    // Cannot 'Back' when one the first page
                    if (_tabControl.SelectedIndex > 0)
                        ret = true;
                    break;
            }

            return ret;
        }

        protected bool ShouldShowCancel()
        {
            bool ret = false;

            switch(_showCancel)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Cancel' on the last page of an Install
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 1))
                                    ret = true;
                            }
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            ret = true;
                            break;
                    }
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableCancel()
        {
            bool ret = false;

            switch(_enableCancel)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        protected bool ShouldShowUpdate()
        {
            bool ret = false;

            switch(_showUpdate)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            break;
                        case Profiles.Configure:
                            break;
                        case Profiles.Controller:
                            ret = true;
                            break;
                    }
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableUpdate()
        {
            bool ret = false;

            switch(_enableUpdate)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        protected bool ShouldEnableHelp()
        {
            bool ret = false;

            switch(_enableCancel)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        protected bool ShouldShowHelp()
        {
            bool ret = false;

            switch(_showUpdate)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    break;
            }

            return ret;
        }

        protected void LeaveFullPage()
        {
            _panelTop.Show();
            _tabControl.Top = _panelTop.Height;
            _tabControl.Height = _panelBottom.Top - _panelTop.Height - 1;
        }
        
        protected void EnterFullPage()
        {
            _panelTop.Hide();
            _tabControl.Top = 0;
            _tabControl.Height = _panelBottom.Top - 1;
        }

        protected void OnTabSelectionChanged(object sender, EventArgs e)
        {
            // Update buttons to reflect change
            UpdateControlButtons();
            
            if (_tabControl.SelectedIndex != -1)
            {
                // Get the selected wizard page
                WizardPage wp = _wizardPages[_tabControl.SelectedIndex];
                
                // Should we be presented in full page?
                if (wp.FullPage)
                    EnterFullPage();
                else
                {
                    // Controller profile is not allowed to be outside of FullMode
                    if (_profile != Profiles.Controller)
                        LeaveFullPage();
                }
            }
            else
            {
                // Controller profile is not allowed to be outside of FullMode
                if (_profile != Profiles.Controller)
                    LeaveFullPage();
            }
            
            // Update manual drawn text
            _panelTop.Invalidate();
            
            // Generate raw selection changed event
            OnSelectionChanged(EventArgs.Empty);
            
            // Generate page leave event if currently on a valid page
            if (_selectedPage != null)
            {
                OnWizardPageLeave(_selectedPage);
                _selectedPage = null;
            }
            
            // Remember which is the newly seleced page
            if (_tabControl.SelectedIndex != -1)
                _selectedPage = _wizardPages[_tabControl.SelectedIndex] as WizardPage;
            
            // Generate page enter event is now on a valid page
            if (_selectedPage != null)
                OnWizardPageEnter(_selectedPage);
        }

        protected void OnButtonHelp(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnHelpClick(EventArgs.Empty);
        }

        protected void OnButtonClose(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnCloseClick(EventArgs.Empty);
        }

        protected void OnButtonFinish(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnFinishClick(EventArgs.Empty);
        }

        protected void OnButtonNext(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs(false);
            
            // Give handlers chance to cancel this action
            OnNextClick(ce);
            
            if (!ce.Cancel)
            {
                // Move to the next page if there is one
                if (_tabControl.SelectedIndex < _tabControl.TabPages.Count - 1)
                    _tabControl.SelectedIndex++;
            }
        }

        protected void OnButtonBack(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs(false);
            
            // Give handlers chance to cancel this action
            OnBackClick(ce);
            
            if (!ce.Cancel)
            {
                // Move to the next page if there is one
                if (_tabControl.SelectedIndex > 0)
                    _tabControl.SelectedIndex--;
            }
        }

        protected void OnButtonCancel(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnCancelClick(EventArgs.Empty);
        }

        protected void OnButtonUpdate(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnUpdateClick(EventArgs.Empty);
        }

        protected void OnWizardCleared()
        {
            // Unhook from event handlers for each page
            foreach(WizardPage wp in _tabControl.TabPages)
            {
                wp.FullPageChanged -= new EventHandler(OnWizardFullPageChanged);
                wp.SubTitleChanged -= new EventHandler(OnWizardSubTitleChanged);
                wp.CaptionTitleChanged -= new EventHandler(OnWizardCaptionTitleChanged);
            }
        
            // Reflect change on underlying tab control
            _tabControl.TabPages.Clear();

            // Update buttons to reflect status
            UpdateControlButtons();
        }
        
        protected void OnWizardInserted(int index, object value)
        {
            WizardPage wp = value as WizardPage;
           
           // Monitor property changes
           wp.FullPageChanged += new EventHandler(OnWizardFullPageChanged);
           wp.SubTitleChanged += new EventHandler(OnWizardSubTitleChanged);
           wp.CaptionTitleChanged += new EventHandler(OnWizardCaptionTitleChanged);
        
            // Reflect change on underlying tab control
            _tabControl.TabPages.Insert(index, wp);

            // Update buttons to reflect status
            UpdateControlButtons();
        }
        
        protected void OnWizardRemoved(int index, object value)
        {
            WizardPage wp = _tabControl.TabPages[index] as WizardPage;
        
            // Unhook from event handlers
            wp.FullPageChanged -= new EventHandler(OnWizardFullPageChanged);
            wp.SubTitleChanged -= new EventHandler(OnWizardSubTitleChanged);
            wp.CaptionTitleChanged -= new EventHandler(OnWizardCaptionTitleChanged);

            // Reflect change on underlying tab control
            _tabControl.TabPages.RemoveAt(index);

            // Update buttons to reflect status
            UpdateControlButtons();
        }
        
        protected void OnWizardFullPageChanged(object sender, EventArgs e)
        {
            WizardPage wp = sender as WizardPage;
            
            // Is it the current page that has changed FullPage?
            if (_tabControl.SelectedIndex == _wizardPages.IndexOf(wp))
            {
                // Should we be presented in full page?
                if (wp.FullPage)
                    EnterFullPage();
                else
                {
                    // Controller profile is not allowed to be outside of FullMode
                    if (_profile != Profiles.Controller)
                        LeaveFullPage();
                }
            }
        }

        protected void OnWizardSubTitleChanged(object sender, EventArgs e)
        {
            WizardPage wp = sender as WizardPage;
           
            // Is it the current page that has changed sub title?
            if (_tabControl.SelectedIndex == _wizardPages.IndexOf(wp))
            {
                // Force the sub title to be updated now
                _panelTop.Invalidate();
            }
        }        
        
        protected void OnWizardCaptionTitleChanged(object sender, EventArgs e)
        {
            // Generate event so any dialog containing use can be notify
            if (WizardCaptionTitleChanged != null)
                WizardCaptionTitleChanged(this, e);
        }
    
        protected override void OnResize(EventArgs e)
        {
            this.PerformLayout();
        }

        protected void OnRepaintPanels(object sender, EventArgs e)
        {
            _panelTop.Invalidate();
            _panelBottom.Invalidate();
        }

        protected void OnPaintTopPanel(object sender, PaintEventArgs pe)
        {
            int right = _panelTop.Width;
        
            // Any picture to draw?
            if (_picture != null)
            {
                // Calculate starting Y position to give equal space above and below image
                int Y = (int)((_panelTop.Height - _picture.Height) / 2);
                
                pe.Graphics.DrawImage(_picture, _panelTop.Width - _picture.Width - Y, Y, _picture.Width, _picture.Height);
                
                // Adjust right side by width of width and gaps around it
                right -= _picture.Width + Y + _panelGap;
            }
        
            // Create main title drawing rectangle
            RectangleF drawRectF = new Rectangle(_panelGap, _panelGap, right - _panelGap, _fontTitle.Height);
                                                
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Near;
            drawFormat.LineAlignment = StringAlignment.Center;
            drawFormat.Trimming = StringTrimming.EllipsisCharacter;
            drawFormat.FormatFlags = StringFormatFlags.NoClip |
                                     StringFormatFlags.NoWrap;
            
            using(SolidBrush mainTitleBrush = new SolidBrush(_colorTitle))
                pe.Graphics.DrawString(_title, _fontTitle, mainTitleBrush, drawRectF, drawFormat);            
             
            // Is there a selected tab for display?   
            if (_tabControl.SelectedIndex != -1)
            {                
                // Adjust rectangle for rest of the drawing text space
                drawRectF.Y = drawRectF.Bottom + (_panelGap / 2);
                drawRectF.X += _panelGap;
                drawRectF.Width -= _panelGap;
                drawRectF.Height = _panelTop.Height - drawRectF.Y - (_panelGap / 2);

                // No longer want to prevent word wrap to extra lines
                drawFormat.LineAlignment = StringAlignment.Near;
                drawFormat.FormatFlags = StringFormatFlags.NoClip;

                WizardPage wp = _tabControl.TabPages[_tabControl.SelectedIndex] as WizardPage;

                using(SolidBrush subTitleBrush = new SolidBrush(_colorSubTitle))
                    pe.Graphics.DrawString(wp.SubTitle, _fontSubTitle, subTitleBrush, drawRectF, drawFormat);
            }                          
        
            using(Pen lightPen = new Pen(_panelTop.BackColor),
                      darkPen = new Pen(ControlPaint.Light(ControlPaint.Dark(this.BackColor))))
            {
                pe.Graphics.DrawLine(darkPen, 0, _panelTop.Height - 2, _panelTop.Width, _panelTop.Height - 2);
                pe.Graphics.DrawLine(lightPen, 0, _panelTop.Height - 1, _panelTop.Width, _panelTop.Height - 1);
            }            
        }
        
        protected void OnPaintBottomPanel(object sender, PaintEventArgs pe)
        {
            using(Pen lightPen = new Pen(ControlPaint.Light(this.BackColor)),
                      darkPen = new Pen(ControlPaint.Light(ControlPaint.Dark(this.BackColor))))
            {
                pe.Graphics.DrawLine(darkPen, 0, 0, _panelBottom.Width, 0);
                pe.Graphics.DrawLine(lightPen, 0, 1, _panelBottom.Width, 1);
            }            
        }
    }
}
