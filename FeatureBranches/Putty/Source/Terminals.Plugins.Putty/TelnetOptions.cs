using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    public class TelnetOptions : PuttyOptions
    {

        public override ProtocolOptions Copy()
        {
            return new TelnetOptions()
            {
                SessionName = this.SessionName
            };
        }
    }
}
