using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data.Credentials;

namespace Terminals.Data
{
    internal class ModelConvertersTemplate
    {
        private readonly ConnectionManager connectionManager;

        protected IPersistence Persistence { get; private set; }

        protected IGuardedCredentialFactory CredentialFactory { get; private set; }

        protected ModelConvertersTemplate(IPersistence persistence)
        {
            this.Persistence = persistence;
            this.CredentialFactory = new GuardedCredentialFactory(this.Persistence.Security);
            this.connectionManager = ConnectionManager.Instance;
        }

        protected IOptionsConverter CreateOptionsConverter(string protocolName)
        {
            IOptionsConverterFactory converterFactory = this.connectionManager.GetOptionsConverterFactory(protocolName);
            return converterFactory.CreatOptionsConverter();
        }
    }
}