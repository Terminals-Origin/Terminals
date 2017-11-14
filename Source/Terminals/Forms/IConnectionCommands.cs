using Terminals.Data;

namespace Terminals.Forms
{
    internal interface IConnectionCommands
    {
        void Disconnect();

        void Reconnect();

        bool CanExecute(IFavorite selected);
    }
}