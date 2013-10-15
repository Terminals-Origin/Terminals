﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.Validation;

namespace Terminals.Forms
{
    /// <summary>
    /// Custom validation of newly entered values in NewTerminalForm
    /// </summary>
    internal class NewTerminalFormValidator
    {
        private readonly NewTerminalForm form;

        private readonly IPersistence persistence;

        private readonly Dictionary<string, Control> validationBindings = new Dictionary<string, Control>();

        public NewTerminalFormValidator(IPersistence persistence, NewTerminalForm form)
        {
            this.persistence = persistence;
            this.form = form;
        }

        internal void RegisterValidationControl(string propertyName, Control control)
        {
            this.validationBindings.Add(propertyName, control);
        }

        internal bool Validate()
        {
            // once the save button is clicked, force the validation of all controls, 
            // even, if they were already validated, to be able to cancel the save
            bool isValid = this.form.ValidateChildren();
            if (!isValid)
                return false;

            return this.ValidatePersistenceConstraints();
        }

        /// <summary>
        /// call aditional persistence validation, depending on persistence type.
        /// here we do some validations again, may be considered to be improved.
        /// Returns true, if persistence is satisfied; ohterwise false.
        /// </summary>
        private bool ValidatePersistenceConstraints()
        {
            IFavorite favorite = this.persistence.Factory.CreateFavorite();
            this.form.FillFavoriteFromControls(favorite);
            ValidationStates results = Validations.Validate(favorite);
            this.UpdateControlsErrorByResults(results);
            // check the results, not the bindings to be able to identify unbound property errors
            return results.Empty;
        }

        private void UpdateControlsErrorByResults(ValidationStates results)
        {
            // loop through bindings to be able to reset the validaiton state for valid controls
            foreach (KeyValuePair<string, Control> binding in this.validationBindings)
            {
                string message = results[binding.Key];
                this.form.SetErrorInfo(binding.Value, message);
            }
        }

        internal void OnServerNameValidating(object sender, CancelEventArgs eventArgs)
        {
            const string MESSAGE = "Server name is required and has to be valid computer name or IP adress.";
            this.IsValid(sender, eventArgs, this.IsServerNameValid, MESSAGE);
        }

        internal void OnUrlValidating(object sender, CancelEventArgs eventArgs)
        {
            this.IsValid(sender, eventArgs, this.IsUrlValid, "Server URL isnt in valid format.");
        }

        internal void OnPortValidating(object sender, CancelEventArgs eventArgs)
        {
            this.IsValid(sender, eventArgs, this.IsPortValid, "Port must be a number between 0 and 65535");
        }

        private void IsValid(object sender, CancelEventArgs eventArgs, Func<bool> isValid, string message)
        {
            eventArgs.Cancel = !isValid();
            string errorMessage = eventArgs.Cancel ? message : string.Empty;
            var control = sender as Control;
            this.form.SetErrorInfo(control, errorMessage);
        }

        private bool IsServerNameValid()
        {
            string protocol = this.form.ProtocolText;
            if (ConnectionManager.IsProtocolWebBased(protocol))
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

        internal void OnValidateInteger(object sender, CancelEventArgs eventArgs)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;

            int parsedValue;
            bool isPared = int.TryParse(textBox.Text, out parsedValue);
            string errorMessage = isPared ? string.Empty : "Is not a valid integer.";
            eventArgs.Cancel = !isPared;
            this.form.SetErrorInfo(textBox, errorMessage);
        }

        internal bool ValidateGrouName(TextBox txtGroupName)
        {
            string groupName = txtGroupName.Text;
            string message = new GroupNameValidator(this.persistence).ValidateNew(groupName);
            this.form.SetErrorInfo(txtGroupName, message);
            return string.IsNullOrEmpty(message);
        }
    }
}
