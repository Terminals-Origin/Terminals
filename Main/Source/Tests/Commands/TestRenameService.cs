using System;
using Terminals;
using Terminals.Data;

namespace Tests.Commands
{
    /// <summary>
    /// Test Stub which allows to replace
    /// </summary>
    internal class TestRenameService : RenameService
    {
        private readonly Func<string, bool> askToOverWrite;

        public TestRenameService(IFavorites favorites, Func<string, bool> askToOverWrite)
            : base(favorites)
        {
            this.askToOverWrite = askToOverWrite;
        }

        public override bool AskUserIfWantsToOverwrite(string newName)
        {
            return this.askToOverWrite(newName);
        }

        public override void ReportInvalidName(string errorMessage)
        {
            // nothing to do in tests
        }
    }
}