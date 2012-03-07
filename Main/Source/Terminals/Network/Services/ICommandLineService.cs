using System.ServiceModel;
using Terminals.CommandLine;

namespace Terminals.Network
{
    [ServiceContract]
    public interface ICommandLineService
    {
        [OperationContract]
        void ForwardCommand(CommandLineArgs commandArguments);
    }
}
