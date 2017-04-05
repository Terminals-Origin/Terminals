using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    public class TelnetOptions : PuttyOptions
    {

        public override ProtocolOptions Copy()
        {
            var options = new TelnetOptions();
            base.Copy(options);
            return options;
        }
    }
}
