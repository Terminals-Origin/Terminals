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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Controls
{
    [Designer(typeof(ParentControlDesigner))]
    public class WizardPage : Crownwood.Magic.Controls.TabPage
	{
	    // Instance fields
	    protected bool _fullPage;
        protected string _subTitle;
        protected string _captionTitle;
        
        // Instance events
        public event EventHandler FullPageChanged;
        public event EventHandler SubTitleChanged;
        public event EventHandler CaptionTitleChanged;
    
		public WizardPage()
		{
		    _fullPage = false;
		    _subTitle = "(Page Description not defined)";
            _captionTitle = "(Page Title)";
        }
		
		public bool FullPage
		{
		    get { return _fullPage; }
		    
		    set 
		    {
		        if (_fullPage != value)
		        {
		            _fullPage = value;
		            OnFullPageChanged(EventArgs.Empty);
		        }
		    }
		}
		
		public string SubTitle
		{
		    get { return _subTitle; }

		    set 
		    {
		        if (_subTitle != value)
		        {
		            _subTitle = value;
		            OnSubTitleChanged(EventArgs.Empty);
		        }
		    }
		}
		
		public string CaptionTitle
		{
		    get { return _captionTitle; }
		    
		    set
		    {
		        if (_captionTitle != value)
		        {
		            _captionTitle = value;
		            OnCaptionTitleChanged(EventArgs.Empty);
		        }
		    }
		}
		
		public virtual void OnFullPageChanged(EventArgs e)
		{
		    if (FullPageChanged != null)
		        FullPageChanged(this, e);
		}
    
        public virtual void OnSubTitleChanged(EventArgs e)
        {
            if (SubTitleChanged != null)
                SubTitleChanged(this, e);
        }

        public virtual void OnCaptionTitleChanged(EventArgs e)
        {
            if (CaptionTitleChanged != null)
                CaptionTitleChanged(this, e);
        }
    }
}
