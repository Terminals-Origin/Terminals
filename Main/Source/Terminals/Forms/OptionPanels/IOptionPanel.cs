namespace Terminals.Forms
{
    /// <summary>
    /// All panels in Options dialog should implement both methods, without catching exceptions
    /// or setting delay save. The save would be handled for all of them.
    /// </summary>
    internal interface IOptionPanel
    {
        void LoadSettings();
        void SaveSettings();
    }
}
