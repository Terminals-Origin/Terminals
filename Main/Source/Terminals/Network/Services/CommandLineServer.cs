using System;
using System.ServiceModel;

namespace Terminals.Network
{
    internal class CommandLineServer : ServiceHost
    {
        private const string BASE_ADDRESS = "net.pipe://localhost/Terminals.Codeplex.com/CommandLineService";

        /// <summary>
        /// necessary to isolate user server instances on terminal server
        /// </summary>
        private static string UserSpecificAddress
        {
            get
            {
                return string.Format("{0}/{1}", BASE_ADDRESS, WindowsUserIdentifiers.GetCurrentUserSid());
            }
        }

        internal CommandLineServer(MainForm mainForm)
            : base(new CommandLineService(mainForm), new Uri(BASE_ADDRESS))
        
        {
            AddServiceEndpoint(typeof(ICommandLineService), new NetNamedPipeBinding(), UserSpecificAddress);
        }

        internal static ICommandLineService CreateClient()
        {
            var factory = new ChannelFactory<ICommandLineService>(new NetNamedPipeBinding(),
                new EndpointAddress(UserSpecificAddress));
            return factory.CreateChannel();
        }
    }
}
