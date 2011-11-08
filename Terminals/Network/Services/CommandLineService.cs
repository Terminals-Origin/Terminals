using Terminals.CommandLine;

namespace Terminals.Network
{
    internal class CommandLineService : ICommandLineService
    {
        private MainForm mainForm;

        internal CommandLineService(MainForm mainForm)
        {
           this.mainForm = mainForm;
        }

        public void ForwardCommand(CommandLineArgs commandArguments)
        {
            //this.mainForm.
        }
    }
}
