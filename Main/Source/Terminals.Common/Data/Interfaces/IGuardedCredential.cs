namespace Terminals.Data
{
    public interface IGuardedCredential
    {
        string UserName { get; set; }

        string Domain { get; set; }

        string Password { get; set; }

        string EncryptedPassword { get; set; }
    }
}