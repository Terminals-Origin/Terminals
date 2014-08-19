using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    /// <summary>
    /// General contract for all protocol controls
    /// </summary>
    internal interface IProtocolOptionsControl
    {
        // void SetControls();

        void LoadFrom(IFavorite favorite);

        void SaveTo(IFavorite favorite);
    }
}