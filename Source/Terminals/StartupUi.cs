using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Security;

namespace Terminals
{
    internal class StartupUi : IStartupUi
    {
        public bool UserWantsFallback()
        {
            const string MESSAGE = "Do you wan't to start with local files store?\r\n" +
                                   "Yes - start with files store (You will be able to fix the configuration.)\r\n" +
                                   "No - Exit application";
            DialogResult fallback = MessageBox.Show(MESSAGE, "Terminals database connection failed",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            return fallback == DialogResult.Yes;
        }

        public AuthenticationPrompt KnowsUserPassword(bool previousTrySuccess)
        {
            return RequestPassword.KnowsUserPassword(previousTrySuccess);
        }

        public void Exit()
        {
            Environment.Exit(-1);
        }
    }
}