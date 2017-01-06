using System.Windows.Forms;

namespace Terminals.Forms
{
    internal partial class YesNoDisableForm : Form
    {
        private YesNoDisableForm()
        {
            InitializeComponent();
        }

        internal static YesNoDisableResult ShowDialog(string title, string message)
        {
            using (var dialog = new YesNoDisableForm())
            {
                dialog.Text = title;
                dialog.textBoxMessage.Text = message;
                dialog.ShowDialog();
                return new YesNoDisableResult(dialog.DialogResult, dialog.checkBoxDisable.Checked);
            }
        }
    }
}
