namespace Terminals.Data
{
    /// <summary>
    /// Container for user input when asking him for master password
    /// </summary>
    public class AuthenticationPrompt
    {
        /// <summary>
        /// Gets or sets the password entered by user
        /// </summary>
        internal string Password { get; set; }

        /// <summary>
        /// Gets or sets the flag identifying, that the user has canceled the prompt dialog
        /// </summary>
        internal bool Canceled { get; set; }
    }
}