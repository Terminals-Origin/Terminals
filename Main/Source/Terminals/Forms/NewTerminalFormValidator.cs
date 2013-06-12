using System;
using System.ComponentModel.DataAnnotations;
using Terminals.Connections;
using Terminals.Data.Validation;

namespace Terminals.Forms
{
    /// <summary>
    /// Custom validation of newly entered values in NewTerminalForm
    /// </summary>
    internal class NewTerminalFormValidator
    {
        private readonly NewTerminalForm form;

        public NewTerminalFormValidator(NewTerminalForm form)
        {
            this.form = form;
        }

        internal bool HasErrors()
        {
            return !this.IsServerNameValid(this.form.ProtocolText, this.form.ServerNameText) ||
                   !this.IsPortValid(this.form.PortText);
        }

        internal bool IsServerNameValid(string protocolToSet, string serverName)
        {
            if (ConnectionManager.IsProtocolWebBased(protocolToSet))
                return true;

            return this.IsServerNameValid(serverName.Trim());
        }

        private bool IsServerNameValid(string serverName)
        {
            if (IsServerNameEmpty(serverName))
            {
                this.form.RerportErrorInServerName("Server name was not specified.");
                return false;
            }

            if (CustomValidationRules.IsValidServerName(serverName) != ValidationResult.Success)
            {
                this.form.RerportErrorInServerName("Server name is not in the correct format.");
                return false;
            }

            return true;
        }

        internal bool IsServerNameEmpty(string serverName)
        {
            return String.IsNullOrEmpty(serverName);
        }

        private bool IsPortValid(string portText)
        {
            Int32 result;
            bool isPortValid = Int32.TryParse(portText, out result) && result >= 0 && result <= 65535;

            if (isPortValid)
                return true;

            this.form.ShowErrorMessageBox("Port must be a number between 0 and 65535");
            return false;
        }

        internal bool IsUrlValid()
        {
            Uri url = this.form.GetFullUrlFromHttpTextBox();
            bool urlValid = url != null;
            return urlValid;
        }
    }
}
