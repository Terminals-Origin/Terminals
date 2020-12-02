using System;
using Terminals.Data;

namespace Terminals.Plugins.WinBox
{
    /// <summary>
    /// Options container for Http or Https. To obtain absolute url
    /// when retrieving connection use following form:
    /// Favorite_Protocol://Favorite_ServerName:Favorite_Port/WebOptions_RelativeUrl
    /// </summary>
    [Serializable]
    public class WinBoxOptions : ProtocolOptions
    {
        public override ProtocolOptions Copy()
        {
            return new WinBoxOptions();
        }
    }
}
