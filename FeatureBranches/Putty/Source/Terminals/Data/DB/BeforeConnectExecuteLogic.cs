namespace Terminals.Data.DB
{
    internal partial class DbBeforeConnectExecute : IBeforeConnectExecuteOptions
    {
        internal void UpdateFrom(DbBeforeConnectExecute source)
        {
            this.Execute = source.Execute;
            this.Command = source.Command;
            this.CommandArguments = source.CommandArguments;
            this.InitialDirectory = source.InitialDirectory;
            this.WaitForExit = source.WaitForExit;
        }
    }
}
