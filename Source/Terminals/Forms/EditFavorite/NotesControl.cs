using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class NotesControl : UserControl
    {
        public NotesControl()
        {
            InitializeComponent();
        }

        internal void SettErrorProviderIconsAlignment(ErrorProvider errorProvider)
        {
            errorProvider.SetIconAlignment(this.NotesTextbox, ErrorIconAlignment.TopLeft);
        }

        internal void RegisterValidations(NewTerminalFormValidator validator)
        {
            validator.RegisterValidationControl("Notes", this.NotesTextbox);
        }

        internal void SaveTo(IFavorite favorite)
        {
            favorite.Notes = this.NotesTextbox.Text;
        }

        internal void LoadFrom(IFavorite favorite)
        {
            this.NotesTextbox.Text = favorite.Notes;
        }
    }
}
