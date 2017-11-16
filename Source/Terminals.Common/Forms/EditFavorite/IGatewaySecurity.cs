using Terminals.Common.Data.Interfaces;
using Terminals.Configuration;

namespace Terminals.Common.Forms.EditFavorite
{
    public interface IGatewaySecurity
    {
        void InitSecurityPanel(ISecurityService service, IMRUSettings settings);
    }
}