using System;
using System.ComponentModel;
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

        internal void OnServerNameValidating(object sender, CancelEventArgs eventArgs)
        {
            const string MESSAGE = "Server name is required and has to be valid computer name or IP adress.";
            this.IsValid(this.IsServerNameValid, sender, MESSAGE);
        }

        internal void OnUrlValidating(object sender, CancelEventArgs eventArgs)
        {
            this.IsValid(this.IsUrlValid, sender, "Server URL isnt in valid format.");
        }

        internal void OnPortValidating(object sender, CancelEventArgs eventArgs)
        {
           this.IsValid(this.IsPortValid, sender, "Port must be a number between 0 and 65535");
        }

        private void IsValid(Func<bool> isValid, object sender, string message)
        {
            string errorMessage = isValid() ? string.Empty : message;
            this.form.SetErrorInfo(sender, errorMessage);
        }

        internal bool IsServerNameValid()
        {
            string protocol = this.form.ProtocolText;
            if(ConnectionManager.IsProtocolWebBased(protocol))
                return true;
            
            return this.ServerNameInvalid();
        }

        private bool ServerNameInvalid()
        {
            string serverName = this.form.ServerNameText;
            return !IsServerNameEmpty() || CustomValidationRules.IsValidServerName(serverName) == ValidationResult.Success;
        }

        internal bool IsServerNameEmpty()
        {
            return String.IsNullOrEmpty(this.form.ServerNameText);
        }

        private bool IsPortValid()
        {
            Int32 result;
            bool parsed = Int32.TryParse(this.form.PortText, out result);
            return parsed && result >= 0 && result <= 65535;
        }

        internal bool IsUrlValid()
        {
            Uri url = this.form.GetFullUrlFromHttpTextBox();
            return url != null;
        }
    }
}
