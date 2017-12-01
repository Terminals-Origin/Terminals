namespace Terminals.Connections
{
    /// <summary>
    /// Required interation by Connections
    /// </summary>
    public interface IConnectionMainView
    {
        string GetDesktopShare();

        void OnLeavingFullScreen();
    }
}