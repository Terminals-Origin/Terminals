using System;

namespace Terminals.Data
{
    [Serializable]
    public class BeforeConnectExecuteOptions
    {
        public Boolean Execute { get; set; }
        public String Command { get; set; }
        public String ExecuteBeforeConnectArgs { get; set; }
        public String InitialDirectory { get; set; }
        public Boolean WaitForExit { get; set; }
    }
}
