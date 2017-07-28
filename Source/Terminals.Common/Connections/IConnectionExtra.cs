namespace Terminals.Connections
{
    /// <summary>
    /// In reality this is Rdp client facade to isolate the reference to direct usage of the COM class.
    /// </summary>
    public interface IConnectionExtra : IFocusable
    {
        bool FullScreen { get; set; }

        string Server { get; }

        string UserName { get; }

        string Domain { get; }

        bool ConnectToConsole { get; }
    }
}
