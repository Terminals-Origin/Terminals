using System.Windows.Forms;

namespace Terminals.Forms
{
    /// <summary>
    /// General purpose password change dialog,
    /// where user enters old and two times new password. Checks, if the new password confirm matches
    /// </summary>
    internal partial class ChangePassword : Form
    {
        internal string OldPassword
        {
            get
            {
                return this.txtOldPassword.Text;
            }
        }

        internal string NewPassword
        {
            get
            {
                return this.txtNewPassword.Text;
            }
        }

        private bool ConfirmedPasswordMatch
        {
            get
            {
                return this.DialogResult == DialogResult.OK &&
                       this.txtConfirmPassword.Text == this.txtNewPassword.Text;
            }
        }

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void ChangePassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.ConfirmedPasswordMatch)
            {
                e.Cancel = true;
                MessageBox.Show("New password doesnt match", "Database password change",
                                     MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
