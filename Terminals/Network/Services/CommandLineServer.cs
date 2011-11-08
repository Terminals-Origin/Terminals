using System;
using System.ServiceModel;

namespace Terminals.Network
{
    internal class CommandLineServer
    {
        private const string BASE_ADDRESS = "net.pipe://localhost/Terminals.Codeplex.com/CommandLineService";
        private ServiceHost serviceHost;

        #region Thread safe singleton

        private CommandLineServer(){}

        internal static CommandLineServer Instance
        {
            get { return Nested.instance; }
        }

        private static class Nested
        {
            internal static readonly CommandLineServer instance = new CommandLineServer();
        }

        #endregion

        internal void StartServer(MainForm mainForm)
        {
            var service = new CommandLineService(mainForm);
            serviceHost = new ServiceHost(service, new Uri(BASE_ADDRESS));
            serviceHost.AddServiceEndpoint(typeof(ICommandLineService), new NetNamedPipeBinding(), BASE_ADDRESS);
            serviceHost.Open();
        }

        internal static ICommandLineService CreateClient()
        {
            var factory = new ChannelFactory<ICommandLineService>(new NetNamedPipeBinding(),
              new EndpointAddress(BASE_ADDRESS));
            return factory.CreateChannel();
        }
    }
}
