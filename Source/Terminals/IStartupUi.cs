using Terminals.Data;

namespace Terminals
{
    public interface IStartupUi
    {
        bool UserWantsFallback();

        AuthenticationPrompt KnowsUserPassword(bool previousTrySuccess);

        void Exit();
    }
}