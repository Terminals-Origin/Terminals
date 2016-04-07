using Terminals.Data;

namespace Terminals.Common.Forms.EditFavorite
{
    public interface ISupportsSecurityControl
    {
         IGuardedCredentialFactory CredentialFactory { get; set; }
    }
}