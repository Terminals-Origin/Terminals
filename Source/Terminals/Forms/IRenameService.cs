using Terminals.Data;

namespace Terminals
{
    internal interface IRenameService
    {
        void Rename(IFavorite favorite, string newName);

        bool AskUserIfWantsToOverwrite(string newName);

        void ReportInvalidName(string newName);
    }
}