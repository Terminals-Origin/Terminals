namespace Terminals.Data.DB
{
    internal partial class BeforeConnectExecute : IBeforeConnectExecuteOptions
    {
        internal BeforeConnectExecute Copy()
        {
            return new BeforeConnectExecute
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
