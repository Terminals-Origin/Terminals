using System;

namespace Terminals.Data
{
    [Serializable]
    public class BeforeConnectExecuteOptions
    {
        public Boolean Execute { get; set; }
        public String Command { get; set; }
        public String CommandArguments { get; set; }
        public String InitialDirectory { get; set; }
        public Boolean WaitForExit { get; set; }

        internal BeforeConnectExecuteOptions Copy()
        {
            return new BeforeConnectExecuteOptions
                {
                    Execute = this.Execute,
                    Command = this.Command,
                    CommandArguments = this.CommandArguments,
                    InitialDirectory = this.InitialDirectory,
                    WaitForExit = this.WaitForExit
                };
        }
    }
}
