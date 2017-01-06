namespace Terminals.Data
{
    public interface IGuardedCredential
    {
        /// <summary>
        /// Gets or sets the user name in not encrypted form. This value isn't stored.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the domain in not encrypted form. This value isn't stored.
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// Gets or sets the password in not encrypted form. This value isn't stored.
        /// </summary>
        string Password { get; set; }

        string EncryptedPassword { get; set; }
    }
}