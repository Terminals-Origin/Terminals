using System.Collections.Generic;

namespace Terminals.Forms.EditFavorite
{
    /// <summary>
    /// Contract to isolate Rdp local resources control from external dialogs
    /// </summary>
    internal interface IRdpLocalResourcesControl
    {
        List<string> RedirectedDrives { get; set; }

        bool RedirectDevices { get; set; }
    }
}