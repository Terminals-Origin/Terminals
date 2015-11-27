using System.Collections.Generic;
using System.Linq;
using Terminals.Connections;

namespace Terminals.Network
{
    internal class ServiceDetector
    {
        private readonly IConnectionManager connectionManager;

        internal ServiceDetector(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        internal string ResolveServiceName(string iPAddress, int port)
        {
            var plugins = this.connectionManager.GetPluginsByPort(port)
                .ToList();

            if (plugins.Count > 1)
                return ResolveByExtraDetection(iPAddress, port, plugins);

            return plugins.First().PortName;
        }

        private string ResolveByExtraDetection(string iPAddress, int port, List<IConnectionPlugin> plugins)
        {
            var extraDetections = plugins.OfType<IExtraDetection>().ToList();

            var firtSuccessfull = extraDetections.FirstOrDefault(p => p.IsValid(iPAddress, port)) as IConnectionPlugin;
            if (firtSuccessfull != null)
                return firtSuccessfull.PortName;
            
            var standardPlugins = plugins.Except(extraDetections.Cast<IConnectionPlugin>());
            return standardPlugins.First().PortName;
        }
    }
}