using System;
using System.ServiceModel;

namespace Terminals.Network
{
    internal class CommandLineServer : ServiceHost
    {
        private const string BASE_ADDRESS = "net.pipe://localhost/Terminals.Codeplex.com/CommandLineService";

        internal CommandLineServer(MainForm mainForm)
            : base(new CommandLineService(mainForm), new Uri(BASE_ADDRESS))
        
        {
            AddServiceEndpoint(typeof(ICommandLineService), new NetNamedPipeBinding(), BASE_ADDRESS);
        }

        internal static ICommandLineService CreateClient()
        {
            var factory = new ChannelFactory<ICommandLineService>(new NetNamedPipeBinding(),
                new EndpointAddress(BASE_ADDRESS));
            return factory.CreateChannel();
        }
    }
}
