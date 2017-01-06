using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Terminals.Integration
{
    internal abstract class Integration<TIntegrationType> where TIntegrationType: IIntegration
    {
        protected Dictionary<string, TIntegrationType> providers;

        protected TIntegrationType FindProvider(string fileName)
        {
            LoadProviders();

            string extension = Path.GetExtension(fileName);
            if (extension == null)
            {
                return default(TIntegrationType);
            }

            string knownExtension = extension.ToLower();

            if (providers.ContainsKey(knownExtension))
                return providers[knownExtension];

            return default(TIntegrationType);
        }

        protected void AddProviderFilter(StringBuilder stringBuilder, TIntegrationType provider)
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Append("|");
            }

            stringBuilder.Append(provider.Name);
            stringBuilder.Append(" (*");
            stringBuilder.Append(provider.KnownExtension);
            stringBuilder.Append(")|*");
            stringBuilder.Append(provider.KnownExtension); // already in lowercase
        }

        protected abstract void LoadProviders();
    }
}
