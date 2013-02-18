using System.Windows.Forms;

namespace Terminals.Forms
{
    internal partial class UnhandledTerminationForm : Form
    {
        private UnhandledTerminationForm()
        {
            InitializeComponent();
        }

        internal static void ShowRipDialog()
        {
            var form = new UnhandledTerminationForm();
            form.ShowDialog();
        }
    }
}
