namespace Terminals.Configuration
{
    public interface IMRUSettings
    {
        string[] MRUDomainNames { get; }

        string[] MRUUserNames { get; }

        void AddDomainMRUItem(string name);

        void AddUserMRUItem(string name);
    }
}