using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data.Credentials;

namespace Terminals.Data
{
    internal class ModelConvertersTemplate
    {
        protected ConnectionManager ConnectionManager { get; private set; }

        protected IPersistence Persistence { get; private set; }

        protected IGuardedCredentialFactory CredentialFactory { get; private set; }

        protected ModelConvertersTemplate(IPersistence persistence, ConnectionManager connectionManager)
        {
            this.Persistence = persistence;
            this.CredentialFactory = new GuardedCredentialFactory(this.Persistence.Security);
            this.ConnectionManager = connectionManager;
        }

        protected IOptionsConverter CreateOptionsConverter(string protocolName)
        {
            IOptionsConverterFactory converterFactory = this.ConnectionManager.GetOptionsConverterFactory(protocolName);
            return converterFactory.CreatOptionsConverter();
        }
    }
}