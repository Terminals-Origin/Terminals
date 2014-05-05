using System.Collections.Generic;

namespace Terminals.Forms.EditFavorite
{
    public interface IRdpLocalResourcesControl
    {
        List<string> RedirectedDrives { get; set; }

        bool RedirectDevices { get; set; }
    }
}