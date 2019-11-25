﻿using System;
using Terminals.Configuration;

namespace Terminals.Data
{
    /// <summary>
    /// Handles user prompt for master password.
    /// </summary>
    internal class AuthenticationSequence
    {
        private readonly Func<string, bool> isMasterPasswordValid;

        private readonly Func<bool, AuthenticationPrompt> knowsUserPassword;

        internal AuthenticationSequence(Func<string, bool> isMasterPasswordValid, Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            this.isMasterPasswordValid = isMasterPasswordValid;
            this.knowsUserPassword = knowsUserPassword;
        }

        internal static bool IsMasterPasswordDefined()
        {
            return !String.IsNullOrEmpty(Settings.Instance.MasterPasswordHash);
        }

        /// <summary>
        /// Check, if provide password is valid, when defined.
        /// Returns true, if password is not defined or is present and was verified.
        /// </summary>
        internal bool AuthenticateIfRequired(string masterPasswordArg = null)
        {
            if (IsMasterPasswordDefined())
                return this.PromptUserToAuthenticate(masterPasswordArg);

            return true;
        }

        private bool PromptUserToAuthenticate(string masterPasswordArg = null)
        {
            AuthenticationPrompt promptResults;
            if (string.IsNullOrEmpty(masterPasswordArg))
            {
                promptResults = this.knowsUserPassword(false);
            }
            else
            {
                promptResults = new AuthenticationPrompt() { Canceled = false, Password = masterPasswordArg };
            }

            bool authenticated = this.IsUserPaswordValid(promptResults);

            while (!(promptResults.Canceled || authenticated))
            {
                promptResults = this.knowsUserPassword(true);
                authenticated = this.IsUserPaswordValid(promptResults);
            }
            return authenticated;
        }

        private bool IsUserPaswordValid(AuthenticationPrompt promptResults)
        {
            if (!promptResults.Canceled)
                return this.isMasterPasswordValid(promptResults.Password);

            return false;
        }
    }
}