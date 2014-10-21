namespace Terminals.Connections
{
    /// <summary>
    /// Required interation by Connections
    /// </summary>
    internal interface IConnectionMainView
    {
        string GetDesktopShare();

        void SetGrabInputCheck(bool newGrabInput);

        void OnLeavingFullScreen();
    }
}