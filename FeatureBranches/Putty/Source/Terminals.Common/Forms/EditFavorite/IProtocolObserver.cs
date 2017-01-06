namespace Terminals.Forms.EditFavorite
{
    public interface IProtocolObserver
    {
        /// <summary>
        /// Used to inform protocol controls, that server name was changed by general properties control.
        /// </summary>
        /// <param name="newServerName">New value entered in the server name text box</param>
        void OnServerNameChanged(string newServerName);
    }
}