using System.Collections.Generic;

namespace Terminals.Connections
{
    internal interface IConnectionManager
    {
        IEnumerable<IConnectionPlugin> GetPluginsByPort(int port);
    }
}