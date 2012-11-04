namespace Terminals.Data.DB
{
    internal partial class BeforeConnectExecute : IBeforeConnectExecuteOptions
    {
        internal void UpdateFrom(BeforeConnectExecute source)
        {
            this.Execute = source.Execute;
            this.Command = source.Command;
            this.CommandArguments = source.CommandArguments;
            this.InitialDirectory = source.InitialDirectory;
            this.WaitForExit = source.WaitForExit;
        }
    }
}
