using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Plugins.Web;

namespace Terminals.Forms.EditFavorite
{
    internal partial class WebControl : UserControl, IProtocolOptionsControl
    {
        public WebControl()
        {
            InitializeComponent();
        }

        public void LoadFrom(IFavorite favorite)
        {
            var webOptions = favorite.ProtocolProperties as WebOptions;
            if (webOptions == null)
                return;

            this.UsernameID.Text = webOptions.UsernameID;
            this.PasswordID.Text = webOptions.PasswordID;
            this.OptionalID.Text = webOptions.OptionalID;
            this.OptionalValue.Text = webOptions.OptionalValue;
            this.SubmitID.Text = webOptions.SubmitID;
            this.EnableHTMLAuth.Checked = webOptions.EnableHTMLAuth;
            this.EnableFormsAuth.Checked = webOptions.EnableFormsAuth;
        }

        public void SaveTo(IFavorite favorite)
        {
            var webOptions = favorite.ProtocolProperties as WebOptions;
            if (webOptions == null)
                return;

            webOptions.UsernameID = this.UsernameID.Text;
            webOptions.PasswordID = this.PasswordID.Text;
            webOptions.OptionalID = this.OptionalID.Text;
            webOptions.OptionalValue = this.OptionalValue.Text;
            webOptions.SubmitID = this.SubmitID.Text;
            webOptions.EnableHTMLAuth = this.EnableHTMLAuth.Checked;
            webOptions.EnableFormsAuth = this.EnableFormsAuth.Checked;
        }
    }
}
