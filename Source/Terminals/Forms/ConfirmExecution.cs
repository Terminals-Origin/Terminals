using System.Windows.Forms;

namespace Terminals.Forms
{
    public partial class ConfirmExecution : Form
    {
        public ConfirmExecution()
        {
            InitializeComponent();
        }

        public void AssignCommand(string commnadWithArgs)
        { 
            this.commandTextBox.Text = commnadWithArgs;
        }
    }
}
